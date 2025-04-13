using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Room.Exceptions;

public class RoomNotEmptyException : DomainException
{
    public RoomNotEmptyException(string message) : base(message)
    {
    }
    public RoomNotEmptyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
