using PoZiomkaDomain.Student.Commands.ResetPassword;

namespace PoZiomkaApi.Requests.Auth;

public record PasswordResetRequest(string Token, string Password)
{
    public ResetPasswordCommand ToResetPasswordCommand() => new(Token, Password);
}
