using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Exceptions;

public class InvalidTokenException : DomainException
{
    public InvalidTokenException(string message) : base(message)
    {
    }

    public InvalidTokenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
