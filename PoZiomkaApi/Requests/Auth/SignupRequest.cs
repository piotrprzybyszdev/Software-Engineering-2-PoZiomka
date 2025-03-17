using PoZiomkaDomain.Student.Commands.SignupStudent;

namespace PoZiomkaApi.Requests.Auth;

public record SignupRequest(string Email, string Password)
{
    public SignupStudentCommand ToSignupStudentByUserCommand() => new(Email, Password, false);
    public SignupStudentCommand ToSignupStudentByAdminCommand() => new(Email, Password, true);
}
