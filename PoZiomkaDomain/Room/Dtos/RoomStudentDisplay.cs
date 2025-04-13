using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Room.Dtos;

public record RoomStudentDisplay(int Id, int Floor, int Number, int Capacity, int? ReservationId, IEnumerable<StudentDisplay> Students)
{
    public RoomStatus Status => ReservationId is not null ? RoomStatus.Reserved :
        !Students.Any() ? RoomStatus.Available : Students.Count() == Capacity ? RoomStatus.Full : RoomStatus.Occupied;
}
