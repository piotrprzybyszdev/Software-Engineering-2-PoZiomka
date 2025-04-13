using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Application.Exceptions;

public class ApplicationAccessException : DomainException
{
    public ApplicationAccessException(string message) : base(message)
    {
    }

    public ApplicationAccessException(string message, Exception innerException) : base(message, innerException)
    {
    }
}