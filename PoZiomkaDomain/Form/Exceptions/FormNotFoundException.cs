using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Form.Exceptions;

public class FormNotFoundException : DomainException
{
    public FormNotFoundException(string message) : base(message)
    {
    }
    public FormNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
