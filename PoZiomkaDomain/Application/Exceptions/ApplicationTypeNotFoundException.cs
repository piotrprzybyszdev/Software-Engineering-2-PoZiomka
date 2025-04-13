using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Application.Exceptions;

public class ApplicationTypeNotFoundException : DomainException
{
    public ApplicationTypeNotFoundException(string message) : base(message)
    {
    }

    public ApplicationTypeNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}