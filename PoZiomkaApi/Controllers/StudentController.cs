using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using static PoZiomkaApi.Authentication;

namespace PoZiomkaApi.Controllers;

[Route("/")]
[ApiController]
public class StudentController(IStudentRepository studentRepository): Controller
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
	//[Authorize] 
	public async Task<StudentModel> GetStudentById(int id)
	{
		var student = await studentRepository.GetStudentById(id, null);

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

