﻿using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Auth;
using PoZiomkaDomain.Common;
using System.Security.Claims;

namespace PoZiomkaApi.Controllers;

[Route("/")]
[ApiController]

public class AuthController(IMediator mediator, IJwtService jwtService, IEmailService emailService, IPasswordService passwordService) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest signupRequest)
    {
        await mediator.Send(signupRequest.ToSignupStudentByUserCommand());
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        IEnumerable<Claim> claims = await mediator.Send(loginRequest.ToLoginStudentCommand());

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

    [Authorize(Roles = Roles.Student)]
    [HttpGet("get-jwt-test")]
    public async Task<IActionResult> GetJwtTest()
    {
        return Ok(await jwtService.GenerateToken(HttpContext.User.Identities.First(), TimeSpan.FromMinutes(20)));
    }

    [Authorize(Roles = Roles.Student)]
    [HttpGet("decode-jwt-test/{token}")]
    public async Task<IActionResult> DecodeJwtTest(string token)
    {
        var identity = await jwtService.ReadToken(token);

        if (identity.HasClaim(ClaimTypes.Role, Roles.Student))
            return Ok("Student");
        if (identity.HasClaim(ClaimTypes.Role, Roles.Administrator))
            return Ok("Admin");

        return Ok("No role found");
    }

    [HttpPost("get-hash")]
    public async Task<IActionResult> GetHash([FromBody] string password)
    {
        return Ok(passwordService.ComputeHash(password));
    }


    [HttpPost("get-email-verification-jwt-test/{email}")]
    public async Task<IActionResult> GetEmailVerificationJwtTest(string email)
    {
        var token = await jwtService.GenerateToken(new ClaimsIdentity([new Claim(ClaimTypes.Email, email)]), TimeSpan.FromMinutes(20));
        return Ok(token);
    }

    [HttpGet("email-test")]
    public async Task<IActionResult> SendEmail(string email)
    {
        await emailService.SendEmailConfirmationEmail(email);
        return Ok();
    }

}
