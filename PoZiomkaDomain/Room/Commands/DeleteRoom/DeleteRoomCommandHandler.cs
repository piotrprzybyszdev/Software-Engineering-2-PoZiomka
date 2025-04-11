using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Room.Commands.DeleteRoom;

public class DeleteRoomCommandHandler(IRoomRepository roomRepository) : IRequestHandler<DeleteRoomCommand>
{
    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
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
