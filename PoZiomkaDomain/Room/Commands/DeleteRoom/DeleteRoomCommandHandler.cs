using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;

namespace PoZiomkaDomain.Room.Commands.DeleteRoom;

public class DeleteRoomCommandHandler(IRoomRepository roomRepository, IStudentRepository studentRepository) : IRequestHandler<DeleteRoomCommand>
{
    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var students = await studentRepository.GetStudentsByRoomId(request.Id, cancellationToken);

        if (students.Count() > 0)
        {
            throw new RoomNotEmptyException("Trying to delete non-empty room");
        }

        try
        {
            await roomRepository.DeleteRoom(request.Id, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new RoomNotFoundException("Room not found", e);
        }
    }
}
