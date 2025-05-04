using MediatR;
using PoZiomkaDomain.Match.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Match.Queries.StudentMatchesQuery;

public record StudentMatchQuery(ClaimsPrincipal User) : IRequest<IEnumerable<MatchModel>>
{
}
