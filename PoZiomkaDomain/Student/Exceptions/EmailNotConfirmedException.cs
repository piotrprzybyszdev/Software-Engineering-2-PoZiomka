using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Exceptions;

public class EmailNotConfirmedException(string message) : DomainException(message);
