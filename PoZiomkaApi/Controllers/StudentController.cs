using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Auth;
using PoZiomkaApi.Requests.Student;
using PoZiomkaApi.Utils;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Student.Commands.DeleteStudent;
using PoZiomkaDomain.Student.Queries.GetAllStudents;
using PoZiomkaDomain.Student.Queries.GetStudent;

namespace PoZiomkaApi.Controllers;

[Route("/student")]
[ApiController]
public class StudentController(IMediator mediator) : Controller
{
    [HttpPut("confirm")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmRequest confirmRequest)
    {
        await mediator.Send(confirmRequest.ToConfirmStudentCommand());
        return Ok();
    }

    [HttpGet("get-logged-in")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> GetLoggedIn()
    {
        int loggedInUserId = User.GetUserId();

        GetStudentQuery getStudent = new(loggedInUserId, User);
        return Ok(await mediator.Send(getStudent));
    }

    [HttpGet("get")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Get()
    {
        GetAllStudentsQuery command = new();
        return Ok(await mediator.Send(command));
    }

    [HttpGet("get/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    [Authorize]
    public async Task<IActionResult> GetStudentById(int id)
    {
        GetStudentQuery getStudent = new(id, User);
        var student = await mediator.Send(getStudent);

        return Ok(student);
    }

    [HttpPost("create")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest createRequest)
    {
        await mediator.Send(createRequest.ToCreateStudentCommand());
        return Ok();
    }

    [HttpPut("update")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentRequest updateRequest)
    {
        await mediator.Send(updateRequest.ToUpdateStudentCommand(HttpContext.User));
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        DeleteStudentCommand deleteStudentCommand = new(id);
        await mediator.Send(deleteStudentCommand);
        return Ok();
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetRequest requestPasswordResetRequest)
    {
        await mediator.Send(requestPasswordResetRequest.ToRequestPasswordResetCommand());

        return Ok();
    }

    [HttpPut("password-reset")]
    public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequest passwordResetRequest)
    {
        await mediator.Send(passwordResetRequest.ToResetPasswordCommand());

        return Ok();
    }
}
