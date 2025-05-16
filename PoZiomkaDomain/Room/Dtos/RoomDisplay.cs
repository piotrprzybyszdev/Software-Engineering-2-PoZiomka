namespace PoZiomkaDomain.Room.Dtos;

public enum RoomStatus
{
    Available, Reserved, Occupied, Full
}

public record RoomDisplay(int Id, int Floor, int Number, int Capacity, int StudentCount, int? ReservationId)
{
    public RoomStatus Status
    {
        get => ReservationId is not null ? RoomStatus.Reserved : StudentCount == 0 ? RoomStatus.Available : StudentCount == Capacity ? RoomStatus.Full  : RoomStatus.Occupied;
    }
};
