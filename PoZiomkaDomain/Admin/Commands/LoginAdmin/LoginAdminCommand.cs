using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Admin.Commands.LoginAdmin;

public record LoginAdminCommand(string Email, string Password) : IRequest<IEnumerable<Claim>>;
