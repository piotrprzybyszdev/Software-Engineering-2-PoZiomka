namespace PoZiomkaDomain.Application.Exceptions;

public class ApplicationTypeNotFoundException : Exception
{
    public ApplicationTypeNotFoundException()
    {
    }

    public ApplicationTypeNotFoundException(string? message) : base(message)
    {
    }

    public ApplicationTypeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}