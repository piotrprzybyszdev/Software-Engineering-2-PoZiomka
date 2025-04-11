using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Room.Commands.AddStudentToRoom;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Room.Commands.AddStudent;

public class AddStudentCommandHandler(IRoomRepository roomRepository, IStudentRepository studentRepository) : IRequestHandler<AddStudentCommand>
{
    public async Task Handle(AddStudentCommand request, CancellationToken cancellationToken)
    {
        StudentModel student;
        try
        {
            student = await studentRepository.GetStudentById(request.StudentId, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new UserNotFoundException("Student not found", e);
        }

        if (student.RoomId is not null)
        {
            throw new StudentAlreadyInRoomException("Student is already in a room");
        }

        RoomModel room;
        try
        {
            room = await roomRepository.GetRoomById(request.Id, cancellationToken);
        }
        catch
        {
            throw new RoomNotFoundException($"Room with id `{request.Id}` not found");
        }

        var students = await studentRepository.GetStudentsByRoomId(request.Id, cancellationToken);

        if (students.Count() >= room.Capacity)
        {
            throw new RoomFullException("Trying to add a student to full room");
        }

        await studentRepository.UpdateRoom(request.StudentId, request.Id);
    }
}
