using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Utils;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Admin.Queries.GetAdmin;

namespace PoZiomkaApi.Controllers;

[Route("/admin")]
[ApiController]
public class AdminController(IMediator mediator) : Controller
{
    [HttpGet("get-logged-in")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> GetLoggedIn()
    {
        var loggedInUserId = User.GetUserId();

        GetAdminQuery getAdmin = new(loggedInUserId, User);
        return Ok(await mediator.Send(getAdmin));
    }
}
