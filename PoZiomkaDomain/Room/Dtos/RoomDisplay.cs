using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Room.Dtos;

public record RoomDisplay(int Id, int Floor, int Number, int Capacity, IEnumerable<StudentDisplay> Students)
{
    public bool IsFull => Students.Count() == Capacity;
};
