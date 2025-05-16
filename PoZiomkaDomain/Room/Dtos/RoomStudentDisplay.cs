using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Room.Dtos;

public record RoomStudentDisplay(int Id, int Floor, int Number, int Capacity, int StudentCount, int? ReservationId, IEnumerable<StudentDisplay> Students)
    : RoomDisplay(Id, Floor, Number, Capacity, StudentCount, ReservationId)
{
}