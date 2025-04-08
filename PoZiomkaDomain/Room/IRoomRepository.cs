using PoZiomkaDomain.Room.Dtos;

namespace PoZiomkaDomain.Room;

public interface IRoomRepository
{
    public Task CreateRoom(RoomCreate roomCreate, CancellationToken? cancellationToken);
}
