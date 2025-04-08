using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Application;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Application.Queries.GetTypes;
using PoZiomkaDomain.Common;
using System.Net.Mime;

namespace PoZiomkaApi.Controllers;

[Route("/application")]
[ApiController]
public class ApplicationController(IMediator mediator) : ControllerBase
{
    [HttpGet("get-types")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> GetTypes()
    {
		var result = await mediator.Send(new GetTypesQuery());
        return Ok(result);
    }

    [HttpGet("get")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Get([FromQuery] GetRequest getRequest)
    {
        return Ok(new List<ApplicationDisplay>([new ApplicationDisplay(
            1, 3, new ApplicationTypeModel(1, "Test application type", "Test application type description"),
            ApplicationStatus.Pending
        )]));
    }

    [HttpGet("get-student")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> GetStudent()
    {
        return Ok(new List<ApplicationDisplay>([new ApplicationDisplay(
            1, 3, new ApplicationTypeModel(1, "Test application type", "Test application type description"),
            ApplicationStatus.Pending
        )]));
    }

    [HttpPost("submit/{id}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Submit(int id, IFormFile file)
    {
        return NotFound();
    }

    [HttpPut("resolve/{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Resolve(int id, ApplicationStatus status)
    {
        return NotFound();
    }

    [HttpGet("download/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> Get(int id)
    {
        return File([0x22], MediaTypeNames.Application.Octet);
    }
}
