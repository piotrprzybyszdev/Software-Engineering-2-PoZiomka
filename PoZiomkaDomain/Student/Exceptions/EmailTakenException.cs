using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Exceptions;

public class EmailTakenException(string message) : DomainException(message)
{
}
