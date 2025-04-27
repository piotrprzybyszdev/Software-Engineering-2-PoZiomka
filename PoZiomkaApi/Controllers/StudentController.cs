using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Auth;
using PoZiomkaApi.Requests.Student;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Student.Commands.DeleteStudent;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student.Queries.GetAllStudents;
using PoZiomkaDomain.Student.Queries.GetStudent;

namespace PoZiomkaApi.Controllers;

[Route("/student")]
[ApiController]
public class StudentController(IMediator mediator) : Controller
{
    [HttpPut("confirm")]
    public async Task<IActionResult> Confirm(ConfirmRequest confirmRequest)
    {
        await mediator.Send(confirmRequest.ToConfirmStudentCommand());
        return Ok();
    }

    [HttpGet("get-logged-in")]
    [Authorize(Roles = Roles.Student)]
    public async Task<StudentDisplay> GetLoggedIn()
    {
        GetStudentQuery getStudent = new(null, User);
        return await mediator.Send(getStudent);
    }

    [HttpGet("get")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IEnumerable<StudentDisplay>> Get()
    {
        GetAllStudentsQuery command = new();
        return await mediator.Send(command);
    }

    [HttpGet("get/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<StudentDisplay> GetStudentById(int id)
    {
        GetStudentQuery getStudent = new(id, User);
        return await mediator.Send(getStudent);
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
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        DeleteStudentCommand deleteStudentCommand = new(id, HttpContext.User);
        await mediator.Send(deleteStudentCommand);

        if (User.IsInRole(Roles.Student))
            await HttpContext.SignOutAsync();

        return Ok();
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetRequest requestPasswordResetRequest)
    {
        await mediator.Send(requestPasswordResetRequest.ToRequestPasswordResetCommand());
        return Ok();
    }

    [HttpPut("password-reset")]
    public async Task<IActionResult> PasswordReset(PasswordResetRequest passwordResetRequest)
    {
        await mediator.Send(passwordResetRequest.ToResetPasswordCommand());
        return Ok();
    }
}
