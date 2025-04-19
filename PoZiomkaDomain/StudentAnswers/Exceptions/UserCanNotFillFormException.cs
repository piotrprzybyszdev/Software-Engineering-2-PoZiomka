namespace PoZiomkaDomain.StudentAnswers.Exceptions;

public class UserCanNotFillFormException : Exception
{
    public UserCanNotFillFormException()
    {
    }

    public UserCanNotFillFormException(string? message) : base(message)
    {
    }

    public UserCanNotFillFormException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}