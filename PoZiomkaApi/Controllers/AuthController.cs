using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Auth;
using System.Security.Claims;

namespace PoZiomkaApi.Controllers;

[Route("/")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupRequest signupRequest)
    {
        await mediator.Send(signupRequest.ToSignupStudent());
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        IEnumerable<Claim> claims = await mediator.Send(loginRequest.ToLoginStudentCommand());

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        await HttpContext.SignInAsync(principal);

        return Ok();
    }

    [HttpPost("login-as-student-mockup")]
    public async Task<IActionResult> LoginAsStudentMockup()
    {
        LoginRequest loginRequest = new LoginRequest("student@example.com", "asdf");
        IEnumerable<Claim> claims = await mediator.Send(loginRequest.ToLoginStudentCommand());

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        await HttpContext.SignInAsync(principal);

        return Ok();
    }

    [HttpPost("admin-login")]
    public async Task<IActionResult> AdminLogin(AdminLoginRequest loginRequest)
    {
        IEnumerable<Claim> claims = await mediator.Send(loginRequest.ToLoginAdminCommand());

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        await HttpContext.SignInAsync(principal);

        return Ok();
    }

    [HttpPost("admin-login-mockup")]
    public async Task<IActionResult> LoginAsAdminMockup()
    {
        AdminLoginRequest loginRequest = new AdminLoginRequest("admin@example.com", "asdf");
        IEnumerable<Claim> claims = await mediator.Send(loginRequest.ToLoginAdminCommand());

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        await HttpContext.SignInAsync(principal);

        return Ok();
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}
