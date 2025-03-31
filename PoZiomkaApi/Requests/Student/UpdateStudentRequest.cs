using PoZiomkaDomain.Student.Commands.UpdateStudent;
using System.Security.Claims;

namespace PoZiomkaApi.Requests.Student;

public record UpdateStudentRequest(int Id, string? FirstName, string? LastName, string? PhoneNumber, string? IndexNumber, bool IsPhoneNumberHidden, bool IsIndexNumberHidden)
{
    public UpdateStudentCommand ToUpdateStudentCommand(ClaimsPrincipal user) => new(user, Id, FirstName, LastName, PhoneNumber, IndexNumber, IsPhoneNumberHidden, IsIndexNumberHidden);
}
