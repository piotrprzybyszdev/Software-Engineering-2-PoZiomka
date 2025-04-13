using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Admin.Exceptions;

public class AdminNotFoundException : DomainException
{
    public AdminNotFoundException(string message) : base(message)
    {
    }
    public AdminNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
