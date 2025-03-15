using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Auth;

namespace PoZiomkaApi.Controllers;

[Route("/student")]
[ApiController]
public class StudentController(IMediator mediator) : ControllerBase
{
    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmRequest confirmRequest)
    {
        await mediator.Send(confirmRequest.ToConfirmStudentCommand());
        return Ok();
    }
}
