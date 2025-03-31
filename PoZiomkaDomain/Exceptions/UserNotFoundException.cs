namespace PoZiomkaDomain.Exceptions;

public class UserNotFoundException : DomainException
{
    public UserNotFoundException(string message) : base(message)
    {
    }
    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
