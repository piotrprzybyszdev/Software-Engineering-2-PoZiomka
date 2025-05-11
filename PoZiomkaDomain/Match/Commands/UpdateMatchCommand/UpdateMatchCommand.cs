using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Match.Commands.UpdateMatchCommand;

public record UpdateMatchCommand(ClaimsPrincipal User, int Id, bool isAccepted) : IRequest
{
}
