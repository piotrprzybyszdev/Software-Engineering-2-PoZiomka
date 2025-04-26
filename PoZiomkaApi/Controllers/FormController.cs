using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Form;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Form.Commands;
using PoZiomkaDomain.Form.Commands.DeleteForm;
using PoZiomkaDomain.Form.Commands.GetAllForms;
using PoZiomkaDomain.Form.Commands.GetForm;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaApi.Controllers;

[Route("/form")]
[ApiController]
public class FormController(IMediator mediator) : ControllerBase
{
    [HttpGet("get")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IEnumerable<FormModel>> Get()
    {
        return await mediator.Send(new GetAllFormsQuery());
    }

    [HttpGet("get-content/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<FormDisplay> GetContent(int id)
    {
        return await mediator.Send(new GetFormQuery(id));
    }

    [HttpPost("create")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Create([FromBody] FormCreateRequest createRequest)
    {
        await mediator.Send(createRequest.ToCreateFormCommand());
        return Ok();
    }

    [HttpPut("update")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update([FromBody] FormUpdateRequest updateRequest)
    {
        return NotFound();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteFormCommand(id));
        return Ok();
    }
}
