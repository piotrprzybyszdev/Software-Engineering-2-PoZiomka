namespace PoZiomkaDomain.Exceptions;

public class EmailTakenException(string message) : DomainException(message)
{
}
