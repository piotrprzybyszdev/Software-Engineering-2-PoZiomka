using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.StudentAnswer;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.StudentAnswers.Commands.Delete;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaDomain.StudentAnswers.Queries.GetAnswer;
using PoZiomkaDomain.StudentAnswers.Queries.GetStudent;

namespace PoZiomkaApi.Controllers;

[Route("/answer")]
[ApiController]
public class StudentAnswerController(IMediator mediator) : ControllerBase
{
    [HttpGet("get-student")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IEnumerable<StudentAnswerStatus>> GetStudent()
    {
        GetStudentAnswersQuery query = new(User);
        return await mediator.Send(query);
    }

    [HttpGet("get/{formId}/{studentId}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<StudentAnswerDisplay> Get(int formId, int studentId)
    {
        GetAnswerQuery query = new(User, formId, studentId);
        return await mediator.Send(query);
    }

    [HttpPost("create")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Create([FromBody] StudentAnswerCreateRequest createRequest)
    {
        var command = createRequest.ToCreateCommand(User);
        await mediator.Send(command);
        return Ok();
    }

    [HttpPut("update")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Update([FromBody] StudentAnswerUpdateRequest updateRequest)
    {
        var command = updateRequest.ToUpdateCommand(User);
        await mediator.Send(command);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteCommand(User, id);
        await mediator.Send(command);
        return Ok();
    }
}
