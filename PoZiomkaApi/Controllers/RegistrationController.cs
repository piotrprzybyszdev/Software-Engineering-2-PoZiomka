using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Auth;
using PoZiomkaApi.Services;
using PoZiomkaDomain.Student;

namespace PoZiomkaApi.Controllers;

[Route("/registration")]
[ApiController]
public class RegistrationController(ISecurityService securityService) : Controller
{
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] Student student)
	{
		return Ok();
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody]LoginReqest loginReqest)
	{
		return Ok();
	}
}
