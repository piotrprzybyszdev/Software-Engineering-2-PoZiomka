namespace PoZiomkaDomain.Exceptions;

public class NoRowsEditedException : DomainException
{
    public NoRowsEditedException(string message) : base(message)
    {
    }
    public NoRowsEditedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}