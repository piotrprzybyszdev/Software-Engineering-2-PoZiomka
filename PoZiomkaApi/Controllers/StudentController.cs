using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using static PoZiomkaApi.Authentication;
using System.Security.Claims;
using PoZiomkaDomain.Match;


namespace PoZiomkaApi.Controllers;

[Route("/")]
[ApiController]
public class StudentController(IStudentRepository studentRepository, IJudgeService judgeService): Controller
{
	[HttpPut("confirm")]
	public async Task<IActionResult> Confirm()
	{
		return NoContent();
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

		return NoContent();
	}

	[HttpGet("get")]
	[Authorize(Roles = Roles.Administrator)]
	public async Task<IActionResult> Get()
	{
		return NoContent();
	}

	[HttpGet("get/{id}")]
	[Authorize] 
	public async Task<StudentDisplay> GetStudentById(int id)
	{
		int loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

		bool ok = User.IsInRole(Roles.Administrator) ||
		  (User.IsInRole(Roles.Student) &&
		  (loggedInUserId == id || await judgeService.IsMatch(loggedInUserId, id)));

		if (!ok)
			throw new UnauthorizedAccessException();

		var student = await studentRepository.GetStudentById(id, null);

		return student.ToStudentDisplay(true);
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

