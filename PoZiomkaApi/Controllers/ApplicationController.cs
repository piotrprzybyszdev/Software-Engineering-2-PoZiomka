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
    [HttpGet("get-types")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> GetTypes()
    {
        return Ok(new List<ApplicationTypeModel>([new ApplicationTypeModel(1, "Test application type", "Test application type description")]));
    }

    [HttpPut("resolve")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Resolve([FromBody] ResolveRequest resolveRequest)
    {
        return NotFound();
    }

    [HttpGet("get")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> Get([FromQuery] GetRequest getRequest)
    {
        return Ok(new List<ApplicationDisplay>([new ApplicationDisplay(
            1, 3, new ApplicationTypeModel(1, "Test application type", "Test application type description"), new NetworkFile(), ApplicationStatus.Pending
        )]));
    }
}
