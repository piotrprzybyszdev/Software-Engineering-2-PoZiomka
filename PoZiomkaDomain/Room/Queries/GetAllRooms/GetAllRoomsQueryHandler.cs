using MediatR;
using PoZiomkaDomain.Room.Dtos;

namespace PoZiomkaDomain.Room.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository) : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomDisplay>>
{
    public async Task<IEnumerable<RoomDisplay>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        return await roomRepository.GetAllRooms(cancellationToken);
    }
}
