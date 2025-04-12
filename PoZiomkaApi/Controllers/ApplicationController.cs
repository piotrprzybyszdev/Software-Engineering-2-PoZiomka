using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Application;
using PoZiomkaDomain.Application.Commands.Submit;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Application.Queries.GetStudent;
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
        var result = await mediator.Send(getRequest.ToQuery());
		return Ok(result);
	}

    [HttpGet("get-student")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> GetStudent()
    {
		var result = await mediator.Send(new GetStudentQuery(User));
		return Ok(result);
	}

    [HttpPost("submit/{id}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Submit(int id, IFormFile file)
    {
		var command = new SubmitCommand(id, (PoZiomkaDomain.Application.IFile)file.OpenReadStream(),User);
		await mediator.Send(command);
		return Ok();
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
