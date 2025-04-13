namespace PoZiomkaDomain.Exceptions;

public class RoomNumberTakenException : DomainException
{
    public RoomNumberTakenException(string message) : base(message)
    {
    }
    public RoomNumberTakenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
