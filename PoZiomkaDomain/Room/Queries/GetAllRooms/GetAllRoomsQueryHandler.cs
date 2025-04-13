using MediatR;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student;

namespace PoZiomkaDomain.Room.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository, IStudentRepository studentRepository) : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomDisplay>>
{
    public async Task<IEnumerable<RoomDisplay>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllRooms(cancellationToken);

        List<RoomDisplay> list = [];

        foreach (var room in rooms)
        {
            var students = await studentRepository.GetStudentIdsByRoomId(room.Id, cancellationToken);
            var roomDisplay = new RoomDisplay(room.Id, room.Floor, room.Number, room.Capacity,
                null, students);
            list.Add(roomDisplay);
        }

        return list;
    }
}
