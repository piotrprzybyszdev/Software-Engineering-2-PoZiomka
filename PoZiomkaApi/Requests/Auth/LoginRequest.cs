using PoZiomkaDomain.Student.Commands.LoginStudent;

namespace PoZiomkaApi.Requests.Auth;

public record LoginRequest(string Email, string Password)
{
    public LoginStudentCommand ToLoginStudentCommand() => new(Email, Password);
}