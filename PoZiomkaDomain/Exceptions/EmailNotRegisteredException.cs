namespace PoZiomkaDomain.Exceptions;

public class EmailNotRegisteredException : DomainException
{
    public EmailNotRegisteredException(string message) : base(message)
    {
    }

    public EmailNotRegisteredException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
