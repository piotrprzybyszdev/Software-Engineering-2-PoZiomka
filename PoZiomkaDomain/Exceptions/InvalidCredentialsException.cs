namespace PoZiomkaDomain.Exceptions;

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException(string message) : base(message)
    {
    }

    public InvalidCredentialsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
