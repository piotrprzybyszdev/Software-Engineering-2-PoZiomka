using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Services;
using PoZiomkaDomain.Form;

namespace PoZiomkaApi.Controllers;

[Route("/user")]
[ApiController]
public class UserController(IFormFiller formFiller, IApplicationService applicationService) : Controller
{
	[HttpPost("fillForm")]
	public async Task<IActionResult> FillForm([FromBody] Answer answers)
	{
		return Ok();
	}

	[HttpPost("makeAnApplication")]
	public async Task<IActionResult> MakeAnApplication([FromBody] string _)
	{
		return Ok();
	}
}

