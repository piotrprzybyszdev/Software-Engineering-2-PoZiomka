using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Room.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student.Exceptions;

namespace PoZiomkaDomain.Room.Commands.RemoveStudent;

public class RemoveStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<RemoveStudentCommand>
{
    public async Task Handle(RemoveStudentCommand request, CancellationToken cancellationToken)
    {
        StudentModel student;
        try
        {
            student = await studentRepository.GetStudentById(request.StudentId, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new StudentNotFoundException("Student not found", e);
        }

        if (student.RoomId != request.Id)
        {
            throw new StudentNotAssignedToRoomException("Student not assigned to this room");
        }

        await studentRepository.UpdateRoom(request.StudentId, null);
    }
}
