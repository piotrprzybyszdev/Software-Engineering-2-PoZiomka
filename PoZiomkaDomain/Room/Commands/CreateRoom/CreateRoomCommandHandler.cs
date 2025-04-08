using MediatR;
using PoZiomkaDomain.Room.Dtos;

namespace PoZiomkaDomain.Room.Commands.CreateRoom;

public class CreateRoomCommandHandler(IRoomRepository roomRepository) : IRequestHandler<CreateRoomCommand>
{
    public async Task Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var roomCreate = new RoomCreate(request.Floor, request.Number, request.Capacity);

        await roomRepository.CreateRoom(roomCreate, cancellationToken);
    }
}
