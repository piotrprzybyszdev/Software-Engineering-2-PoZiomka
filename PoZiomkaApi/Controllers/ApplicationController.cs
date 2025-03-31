using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Common;
using PoZiomkaApi.Requests.Application;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Common;

namespace PoZiomkaApi.Controllers;

[Route("/application")]
[ApiController]
public class ApplicationController(IMediator mediator) : ControllerBase
{
    [HttpPost("submit/{id}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Submit(int id, IFormFile file)
    {
        return NotFound();
    }

    [HttpPut("resolve/{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Resolve([FromBody] ResolveRequest resolveRequest)
    {
        return NotFound();
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        return Ok(new List<ApplicationDisplay>([new ApplicationDisplay(
            1, 3, new ApplicationTypeModel(1, "Test application type", "Test application type description"), new NetworkFile(), ApplicationStatus.Pending
        )]));
    }
}
