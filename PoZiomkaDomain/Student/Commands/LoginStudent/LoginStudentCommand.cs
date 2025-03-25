using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.LoginStudent;

public record LoginStudentCommand(string Email, string Password) : IRequest<IEnumerable<Claim>>;
