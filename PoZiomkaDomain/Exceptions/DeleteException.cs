using PoZiomkaDomain.Exceptions;

public class DeleteException : DomainException
{
    public DeleteException(string message) : base(message)
    {
    }
    public DeleteException(string message, Exception innerException) : base(message, innerException)
    {
    }
}