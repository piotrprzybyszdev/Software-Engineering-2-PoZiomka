using MediatR;

namespace PoZiomkaDomain.Student.Commands.RequestPasswordReset;

public record RequestPasswordResetCommand(string Email) : IRequest;
