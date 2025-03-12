using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Services;
using PoZiomkaDomain.Application;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Communication;

namespace PoZiomkaApi.Controllers;

[Route("/admin")]
[ApiController]
public class AdminController(IApplicationService applicationService, ICommunicationSender communicationSender, IRoomSelector roomSelector) : Controller
{
	[HttpPost("makeAnApplication")]
	public async Task<IActionResult> MakeAnApplication([FromBody] string _)
	{
		return Ok();
	}

	[HttpPost("sendCommunication")]
	public async Task<IActionResult> SendCommunication([FromBody] Communication communication, [FromBody] List<Student> students)
	{
		return Ok();
	}

	[HttpPost("resolveAnApplication")]
	public async Task<IActionResult> ResolveAnApplication([FromBody] Application application)
	{
		return Ok();
	}
}

