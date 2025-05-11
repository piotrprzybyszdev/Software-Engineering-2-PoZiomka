using MediatR;
using PoZiomkaDomain.Match.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Match.Queries.StudentMatchesQuery;

public record StudentMatchQuery(ClaimsPrincipal User) : IRequest<IEnumerable<MatchModel>>
{
}
