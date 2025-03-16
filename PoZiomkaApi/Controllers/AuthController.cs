using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PoZiomkaApi.Requests.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using PoZiomkaDomain.Common;

namespace PoZiomkaApi.Controllers;

[Route("/")]
[ApiController]
public class AuthController(IMediator mediator, IJwtService jwtService, IEmailService emailService) : ControllerBase
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
    [HttpGet("get-jwt-test")]
    public async Task<IActionResult> GetJwtTest()
    {
        return Ok(await jwtService.GenerateToken(HttpContext.User.Identities.First(), TimeSpan.FromMinutes(20)));
    }

    [Authorize(Roles = Authentication.Roles.Student)]
    [HttpGet("decode-jwt-test/{token}")]
    public async Task<IActionResult> DecodeJwtTest(string token)
    {
        var identity = await jwtService.ReadToken(token);

        if (identity.HasClaim(ClaimTypes.Role, Authentication.Roles.Student))
            return Ok("Student");
        if (identity.HasClaim(ClaimTypes.Role, Authentication.Roles.Administrator))
            return Ok("Admin");

        return Ok("No role found");
    }

    [HttpGet("email-test")]
    public async Task<IActionResult> SendEmail(string email)
    {
        await emailService.SendEmailConfirmationEmail(email);
        return Ok();
    }
}
