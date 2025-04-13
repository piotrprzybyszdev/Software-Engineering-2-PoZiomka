using MediatR;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student;

namespace PoZiomkaDomain.Room.Commands.CreateRoom;

public class CreateRoomCommandHandler(IRoomRepository roomRepository) : IRequestHandler<CreateRoomCommand>
{
    public async Task Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var roomCreate = new RoomCreate(request.Floor, request.Number, request.Capacity);
        try
        {
            await roomRepository.CreateRoom(roomCreate, cancellationToken);
        }
        catch (RoomNumberNotUniqueException)
        {
            throw new RoomNumberTakenException($"Room with number `{request.Number}` already exists");
        }
    }
}
