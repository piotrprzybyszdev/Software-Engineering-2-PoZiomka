using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PoZiomkaApi.Requests.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

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

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        var principal = new ClaimsPrincipal(
            new ClaimsIdentity([
                new Claim(ClaimTypes.Role, Authentication.Roles.Student)
            ], CookieAuthenticationDefaults.AuthenticationScheme)
        );

        await HttpContext.SignInAsync(principal);

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

    [Authorize(Roles = Authentication.Roles.Student)]
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        return Ok("Test");
    }
}
