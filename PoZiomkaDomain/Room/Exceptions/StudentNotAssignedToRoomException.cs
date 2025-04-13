using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Room.Exceptions;

public class StudentNotAssignedToRoomException : DomainException
{
    public StudentNotAssignedToRoomException(string message) : base(message)
    {
    }
    public StudentNotAssignedToRoomException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
