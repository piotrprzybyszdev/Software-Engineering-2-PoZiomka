using PoZiomkaDomain.Student.Commands.CreateStudent;

namespace PoZiomkaApi.Requests.Student;

public record CreateStudentRequest(string Email, string? FirstName, string? LastName, string? IndexNumber, string? PhoneNumber)
{
    public CreateStudentCommand ToCreateStudentCommand() => new(Email, FirstName, LastName, IndexNumber, PhoneNumber);  
}
