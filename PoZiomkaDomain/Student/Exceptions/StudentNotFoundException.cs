using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Student.Exceptions;

public class StudentNotFoundException : DomainException
{
    public StudentNotFoundException(string message) : base(message)
    {
    }
    public StudentNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
