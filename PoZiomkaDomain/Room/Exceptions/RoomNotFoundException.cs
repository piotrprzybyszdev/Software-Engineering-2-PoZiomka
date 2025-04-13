using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Room.Exceptions;

public class RoomNotFoundException : DomainException
{
    public RoomNotFoundException(string message) : base(message)
    {
    }
    public RoomNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
