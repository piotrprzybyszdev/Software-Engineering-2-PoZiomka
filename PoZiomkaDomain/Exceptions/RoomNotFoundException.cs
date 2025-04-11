namespace PoZiomkaDomain.Exceptions;

public class RoomNotFoundException : DomainException
{
    public RoomNotFoundException(string message) : base(message)
    {
    }
    public RoomNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
