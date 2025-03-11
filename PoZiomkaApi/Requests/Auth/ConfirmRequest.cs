using PoZiomkaDomain.Student.Commands.ConfirmStudent;

namespace PoZiomkaApi.Requests.Auth;

public record ConfirmRequest(string Email)
{
    public ConfirmStudentCommand ToConfirmStudentCommand() => new(Email);
}