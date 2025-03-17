using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using static PoZiomkaApi.Authentication;
using System.Security.Claims;
using PoZiomkaDomain.Match;
using MediatR;
using PoZiomkaDomain.Student.Commands.GetStudent;
using System.Diagnostics.CodeAnalysis;
using PoZiomkaApi.Requests.Student;
using PoZiomkaApi.Utils;


namespace PoZiomkaApi.Controllers;

[Route("/")]
[ApiController]
public class StudentController(IStudentRepository studentRepository, IJudgeService judgeService, IMediator mediator) : Controller
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
	public async Task<StudentDisplay> GetLoggedIn()
	{
		int loggedInUserId = User.GetUserId();

		GetStudentCommand getStudent = new(loggedInUserId, false);
		return await mediator.Send(getStudent);
	}

	[HttpGet("get")]
	[Authorize(Roles = Roles.Administrator)]
	public async Task<IEnumerable<StudentDisplay>> Get()
	{
		GetAllStudentsCommand getAllStudents = new();
		return await mediator.Send(getAllStudents);
	}

	[HttpGet("get/{id}")]
	[Authorize]
	public async Task<StudentDisplay> GetStudentById(int id)
	{
		// for administrator, for student if is match or is the same student
		int loggedInUserId = User.GetUserId();

		bool ok = User.IsInRole(Roles.Administrator) ||
		  (User.IsInRole(Roles.Student) &&
		  (loggedInUserId == id || await judgeService.IsMatch(loggedInUserId, id)));

		if (!ok)
			throw new UnauthorizedAccessException();
		bool hide = true;
		if (User.IsInRole(Roles.Administrator))
			hide = false;

		GetStudentCommand getStudent = new(id, hide);
		var student = await mediator.Send(getStudent);

		return student;
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


