namespace PoZiomkaDomain.Common.Exceptions;

public class InvalidPasswordException : DomainException
{
    public InvalidPasswordException(string message) : base(message)
    {
    }
    public InvalidPasswordException(string message, Exception innerException) : base(message, innerException)
    {
    }
}