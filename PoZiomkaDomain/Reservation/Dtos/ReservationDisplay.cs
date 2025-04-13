using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Reservation.Dtos;

public record ReservationDisplay(int Id, RoomModel Room, IEnumerable<StudentDisplay> Students, bool IsAcceptedByAdmin)
{
    public bool IsAccepted { get => Students.All(student => student.HasAcceptedReservation ?? false) && IsAcceptedByAdmin; }
};
