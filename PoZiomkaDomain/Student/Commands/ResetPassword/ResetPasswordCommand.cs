using MediatR;

namespace PoZiomkaDomain.Student.Commands.ResetPassword;

public record ResetPasswordCommand(string Token, string Password) : IRequest;
