namespace PoZiomkaDomain.Room.Dtos;

public enum RoomStatus
{
    Available, Reserved, Occupied, Full
}

public record RoomDisplay(int Id, int Floor, int Number, int Capacity, int? ReservationId, IEnumerable<int> StudentIds)
{
    public RoomStatus Status => ReservationId is not null ? RoomStatus.Reserved :
        !StudentIds.Any() ? RoomStatus.Available : StudentIds.Count() == Capacity ? RoomStatus.Full : RoomStatus.Occupied;
};
