using MediatR;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student;

namespace PoZiomkaDomain.Room.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository, IStudentRepository studentRepository) : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomStudentDisplay>>
{
    public async Task<IEnumerable<RoomStudentDisplay>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllRooms(cancellationToken);

        List<RoomStudentDisplay> list = [];

        foreach (var room in rooms)
        {
            var students = await studentRepository.GetStudentsByRoomId(room.Id, cancellationToken);
            var roomStudentDisplay = new RoomStudentDisplay(room.Id, room.Floor, room.Number, room.Capacity,
                null, students.Select(x => x.ToStudentDisplay(false)));
            list.Add(roomStudentDisplay);
        }

        return list;
    }
}
