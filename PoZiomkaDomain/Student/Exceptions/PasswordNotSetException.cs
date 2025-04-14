using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Exceptions;

public class PasswordNotSetException(string message) : DomainException(message);
