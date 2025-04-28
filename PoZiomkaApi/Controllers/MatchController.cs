using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Match;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Match.Dtos;

namespace PoZiomkaApi.Controllers;

[Route("/match")]
[ApiController]
public class MatchController(IMediator mediator) : Controller
{
    [HttpGet("get-student")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IEnumerable<MatchModel>> GetStudentMatches()
    {
        return [
            new MatchModel(1, 1, 2, MatchStatus.Accepted, MatchStatus.Pending),
            new MatchModel(1, 1, 3, MatchStatus.Accepted, MatchStatus.Rejected),
            new MatchModel(1, 1, 4, MatchStatus.Accepted, MatchStatus.Accepted)
        ];
    }

    [HttpPut("update")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> UpdateMatch(MatchUpdateRequest updateRequest)
    {
        return NotFound();
    }
}
