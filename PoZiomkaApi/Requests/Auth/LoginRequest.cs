using PoZiomkaDomain.Student.Commands.LoginStudent;

namespace PoZiomkaApi.Requests.Auth;

public record LoginRequest
{
    /// <example>student@example.com</example>
    public string Email { get; }
    /// <example>asdf</example>
    public string Password { get; }
    public LoginRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
    public LoginStudentCommand ToLoginStudentCommand() => new(Email, Password);
}