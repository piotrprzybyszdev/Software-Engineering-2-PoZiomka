using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Match.Commands.UpdateMatchCommand;

public record UpdateMatchCommand(ClaimsPrincipal User, int Id, bool isAccepted) : IRequest
{
}
