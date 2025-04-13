using System.Runtime.Serialization;

namespace PoZiomkaDomain.Exceptions;

public class ApplicationNotFoundException: Exception
{
    public ApplicationNotFoundException()
    {
    }

    public ApplicationNotFoundException(string? message) : base(message)
    {
    }

    public ApplicationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}