namespace PoZiomkaDomain.Application.Exceptions;

public class ApplicationAccessException : Exception
{
    public ApplicationAccessException()
    {
    }

    public ApplicationAccessException(string? message) : base(message)
    {
    }

    public ApplicationAccessException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}