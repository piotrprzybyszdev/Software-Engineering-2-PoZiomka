using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Common;
using PoZiomkaApi.Requests.Application;
using PoZiomkaDomain.Application.Commands.DownloadApplication;
using PoZiomkaDomain.Application.Commands.ResolveApplication;
using PoZiomkaDomain.Application.Commands.SubmitApplication;
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
    public async Task<IEnumerable<ApplicationTypeModel>> GetTypes()
    {
        return await mediator.Send(new GetTypesQuery());
    }

    [HttpGet("get")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IEnumerable<ApplicationDisplay>> Get([FromQuery] GetRequest getRequest)
    {
        return await mediator.Send(getRequest.ToQuery());
    }

    [HttpGet("get-student")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IEnumerable<ApplicationDisplay>> GetStudent()
    {
        return await mediator.Send(new GetStudentQuery(User));
    }

    [HttpPost("submit/{id}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Submit(int id, IFormFile file)
    {
        if (file.ContentType != MediaTypeNames.Application.Pdf)
            return BadRequest("File must be a PDF");

        var command = new SubmitApplicationCommand(id, new NetworkFile(file), User);
        await mediator.Send(command);
        return Ok();
    }

    [HttpPut("resolve/{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Resolve(int id, ApplicationStatus status)
    {
        var command = new ResolveApplicationCommand(id, status);
        await mediator.Send(command);
        return Ok();
    }

    [HttpGet("download/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<FileStreamResult> Get(int id)
    {
        var command = new DownloadApplicationCommand(id, User);
        var result = await mediator.Send(command);

        return File(result.Stream, MediaTypeNames.Application.Pdf, $"Application-{id}.pdf");
    }
}
