using PoZiomkaDomain.Admin.Commands.LoginAdmin;

namespace PoZiomkaApi.Requests.Auth;

public record AdminLoginRequest
{
    /// <example>admin@example.com</example>
    public string Email { get; }
    /// <example>asdf</example>
    public string Password { get; }
    public AdminLoginRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public LoginAdminCommand ToLoginAdminCommand() => new(Email, Password);
}
