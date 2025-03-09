using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Auth;

namespace PoZiomkaApi.Controllers;

[Route("/")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest signupRequest)
    {
        await mediator.Send(signupRequest.ToSignupStudentCommand());
        return Ok();
    }
}
