using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Communication.Exceptions;

public class UnauthorizedDeleteException : DomainException
{
    public UnauthorizedDeleteException(string message) : base(message)
    {
    }
    public UnauthorizedDeleteException(string message, Exception innerException) : base(message, innerException)
    {
    }
}