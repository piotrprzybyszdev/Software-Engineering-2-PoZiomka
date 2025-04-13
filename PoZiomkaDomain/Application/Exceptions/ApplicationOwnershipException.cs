namespace PoZiomkaDomain.Application.Exceptions;

public class ApplicationOwnershipException : Exception
{
    public ApplicationOwnershipException()
    {
    }

    public ApplicationOwnershipException(string? message) : base(message)
    {
    }

    public ApplicationOwnershipException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}