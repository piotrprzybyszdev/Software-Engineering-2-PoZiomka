using PoZiomkaDomain.Student.Commands.ConfirmStudent;

namespace PoZiomkaApi.Requests.Student;

public record ConfirmRequest(string Token)
{
    public ConfirmStudentCommand ToConfirmStudentCommand() => new(Token);
}