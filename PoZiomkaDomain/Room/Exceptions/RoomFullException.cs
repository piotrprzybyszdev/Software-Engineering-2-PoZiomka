using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Room.Exceptions;

public class RoomFullException : DomainException
{
    public RoomFullException(string message) : base(message)
    {
    }
    public RoomFullException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
