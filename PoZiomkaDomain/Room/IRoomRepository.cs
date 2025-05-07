using PoZiomkaDomain.Room.Dtos;

namespace PoZiomkaDomain.Room;

public class RoomNumberNotUniqueException : Exception;

public interface IRoomRepository
{
    public Task CreateRoom(RoomCreate roomCreate, CancellationToken? cancellationToken);
    public Task<RoomModel> GetRoomById(int id, CancellationToken? cancellationToken);
    public Task<IEnumerable<RoomModel>> GetAllRooms(CancellationToken? cancellationToken);
    public Task DeleteRoom(int id, CancellationToken? cancellationToken);
    public Task<IEnumerable<RoomModel>> GetEmptyRooms();
}
