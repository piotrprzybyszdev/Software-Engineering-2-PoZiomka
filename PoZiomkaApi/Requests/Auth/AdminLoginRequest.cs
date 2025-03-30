using PoZiomkaDomain.Admin.Commands.LoginAdmin;

namespace PoZiomkaApi.Requests.Auth;

public record AdminLoginRequest(string Email, string Password)
{
    public LoginAdminCommand ToLoginAdminCommand() => new(Email, Password);
}
