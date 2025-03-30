namespace PoZiomkaDomain.Exceptions;

public class AdminNotFoundException : DomainException
{
    public AdminNotFoundException(string message) : base(message)
    {
    }

    public AdminNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
