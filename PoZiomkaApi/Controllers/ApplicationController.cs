using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Common;
using PoZiomkaApi.Requests.Application;
using PoZiomkaDomain.Application.Commands.Download;
using PoZiomkaDomain.Application.Commands.Resolve;
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
        if (file.ContentType != MediaTypeNames.Application.Pdf)
            return BadRequest("File must be a PDF");

        var command = new SubmitCommand(id, new NetworkFile(file), User);
        await mediator.Send(command);
        return Ok();
    }

    [HttpPut("resolve/{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Resolve(int id, ApplicationStatus status)
    {
        var command = new ResolveCommand(id, status);
        await mediator.Send(command);
        return Ok();
    }

    [HttpGet("download/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> Get(int id)
    {
        var command = new DownloadCommand(id, User);
        var result = await mediator.Send(command);
        string fileName = $"file_{id}.pdf";
        string contentType = "application/pdf";

        return File(result.Stream, contentType, fileName);
    }
}
