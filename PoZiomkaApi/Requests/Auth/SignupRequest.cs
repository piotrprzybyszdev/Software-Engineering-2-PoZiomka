using PoZiomkaDomain.Student.Commands.SignupStudent;

namespace PoZiomkaApi.Requests.Auth;

public record SignupRequest(string Email, string Password)
{
    public SignupStudentCommand ToSignupStudentCommand() => new(Email, Password);
}
