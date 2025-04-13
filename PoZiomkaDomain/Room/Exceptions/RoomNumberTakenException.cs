using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Room.Exceptions;

public class RoomNumberTakenException : DomainException
{
    public RoomNumberTakenException(string message) : base(message)
    {
    }
    public RoomNumberTakenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
