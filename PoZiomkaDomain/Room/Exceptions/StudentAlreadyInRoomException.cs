using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Room.Exceptions;

public class StudentAlreadyInRoomException : DomainException
{
    public StudentAlreadyInRoomException(string message) : base(message)
    {
    }
    public StudentAlreadyInRoomException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
