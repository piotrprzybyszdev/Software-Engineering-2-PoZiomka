namespace PoZiomkaDomain.Exceptions;

public class TokenValidationException : DomainException
{
    public TokenValidationException(string message) : base(message)
    {
    }

    public TokenValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
