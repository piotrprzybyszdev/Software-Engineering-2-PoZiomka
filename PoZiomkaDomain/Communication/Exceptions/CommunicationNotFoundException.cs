using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Communication.Exceptions;

public class CommunicationNotFoundException : DomainException
{
    public CommunicationNotFoundException(string message) : base(message)
    {
    }
    public CommunicationNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
