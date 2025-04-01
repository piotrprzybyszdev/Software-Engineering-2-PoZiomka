using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Room.Dtos;

public record RoomDisplay(int Id, int Floor, int Number, int Capacity, IEnumerable<int> StudentIds)
{
    public bool IsFull => StudentIds.Count() == Capacity;
};
