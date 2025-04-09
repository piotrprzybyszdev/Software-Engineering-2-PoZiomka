namespace PoZiomkaDomain.Room.Dtos;

public record RoomStudentDisplay(int Id, int Floor, int Number, int Capacity, int? ReservationId, IEnumerable<StudentDisplay> Students)
{
    public RoomStatus Status => ReservationId is not null ? RoomStatus.Reserved :
        !StudentIds.Any() ? RoomStatus.Available : StudentIds.Count() == Capacity ? RoomStatus.Full : RoomStatus.Occupied;
};
