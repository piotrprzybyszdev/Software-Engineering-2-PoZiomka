using PoZiomkaDomain.Room.Dtos;

namespace PoZiomkaDomain.Room;

public class RoomNumberNotUniqueException : Exception;

public interface IRoomRepository
{
    public Task CreateRoom(RoomCreate roomCreate, CancellationToken? cancellationToken);
    public Task<RoomDisplay> GetRoomById(int id, CancellationToken? cancellationToken);
    public Task<IEnumerable<RoomDisplay>> GetAllRooms(CancellationToken? cancellationToken);
    public Task DeleteRoom(int id, CancellationToken? cancellationToken);
    public Task RemoveStudent(int id, int studentId, CancellationToken? cancellationToken);
    public Task AddStudent(int id, int studentId, CancellationToken? cancellationToken);
}
