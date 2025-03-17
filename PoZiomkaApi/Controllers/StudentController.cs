using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Student;
using PoZiomkaApi.Utils;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Student.Commands.GetAllStudent;
using PoZiomkaDomain.Student.Commands.GetStudent;

namespace PoZiomkaApi.Controllers;

[Route("/")]
[ApiController]
public class StudentController(IMediator mediator) : Controller
{
    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmRequest confirmRequest)
    {
        await mediator.Send(confirmRequest.ToConfirmStudentCommand());
        return Ok();
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset()
    {
        return NoContent();
    }

    [HttpGet("get-logged-in")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> GetLoggedIn()
    {
        int loggedInUserId = User.GetUserId();

        GetStudentCommand getStudent = new(loggedInUserId, User);
        return Ok(await mediator.Send(getStudent));
    }

    [HttpGet("get")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Get()
    {
        GetAllStudentsCommand command = new();
        return Ok(await mediator.Send(command));
    }

    [HttpGet("get/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    [Authorize]
    public async Task<IActionResult> GetStudentById(int id)
    {

        int loggedInUserId = User.GetUserId();

        GetStudentCommand getStudent = new(id, User);
        var student = await mediator.Send(getStudent);

        return Ok(student);
    }

    [HttpPost("create")]
    [Authorize(Roles = Roles.Administrator)]
    public IActionResult CreateStudent()
    {
        return NoContent();
    }

    [HttpPut("update")]
    [Authorize]
    public IActionResult UpdateStudent()
    {
        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
    public IActionResult DeleteStudent(int id)
    {
        return NoContent();
    }
}
