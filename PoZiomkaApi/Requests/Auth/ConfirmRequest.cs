using PoZiomkaDomain.Student.Commands.ConfirmStudent;

namespace PoZiomkaApi.Requests.Auth;

public record ConfirmRequest(string Token)
{
    public ConfirmStudentCommand ToConfirmStudentCommand() => new(Token);
}