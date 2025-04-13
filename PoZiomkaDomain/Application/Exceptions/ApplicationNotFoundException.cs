using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Application.Exceptions;

public class ApplicationNotFoundException : DomainException
{
    public ApplicationNotFoundException(string message) : base(message)
    {
    }

    public ApplicationNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}