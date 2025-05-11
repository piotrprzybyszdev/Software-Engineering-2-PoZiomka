using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Match;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Match.Commands.UpdateMatchCommand;
using PoZiomkaDomain.Match.Dtos;
using PoZiomkaDomain.Match.Queries.StudentMatchesQuery;

namespace PoZiomkaApi.Controllers;

[Route("/match")]
[ApiController]
public class MatchController(IMediator mediator) : Controller
{
    [HttpGet("get-student")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IEnumerable<MatchModel>> GetStudentMatches()
    {
        StudentMatchQuery query = new(User);
        return await mediator.Send(query);
    }

    [HttpPut("update")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> UpdateMatch(MatchUpdateRequest updateRequest)
    {
        UpdateMatchCommand command = new(User, updateRequest.Id, updateRequest.IsAcceptation);
        await mediator.Send(command);
        return Ok();
    }
}
