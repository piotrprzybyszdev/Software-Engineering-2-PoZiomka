using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.DeleteStudent;

public record DeleteStudentCommand(int Id, ClaimsPrincipal User) : IRequest;