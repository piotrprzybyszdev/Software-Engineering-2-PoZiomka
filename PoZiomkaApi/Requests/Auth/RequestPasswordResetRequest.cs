using PoZiomkaDomain.Student.Commands.RequestPasswordReset;

namespace PoZiomkaApi.Requests.Auth;

public record RequestPasswordResetRequest(string Email)
{
    public RequestPasswordResetCommand ToRequestPasswordResetCommand() => new(Email);
}
