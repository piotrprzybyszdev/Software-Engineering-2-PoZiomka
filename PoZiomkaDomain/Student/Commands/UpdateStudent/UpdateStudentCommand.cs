using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.UpdateStudent;

public record UpdateStudentCommand(ClaimsPrincipal User, int Id, string? FirstName, string? LastName, string? PhoneNumber, string? IndexNumber, bool IsPhoneNumberHidden, bool IsIndexNumberHidden) : IRequest;
