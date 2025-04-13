using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Application.Exceptions;

public class ApplicationOwnershipException : DomainException
{
    public ApplicationOwnershipException(string message) : base(message)
    {
    }

    public ApplicationOwnershipException(string message, Exception innerException) : base(message, innerException)
    {
    }
}