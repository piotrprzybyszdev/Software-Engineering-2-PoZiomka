using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaDomain.Admin.Dtos;
using PoZiomkaDomain.Admin.Queries.GetAdmin;
using PoZiomkaDomain.Common;

namespace PoZiomkaApi.Controllers;

[Route("/admin")]
[ApiController]
public class AdminController(IMediator mediator) : Controller
{
    [HttpGet("get-logged-in")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<AdminDisplay> GetLoggedIn()
    {
        GetAdminQuery getAdmin = new(User);
        return await mediator.Send(getAdmin);
    }
}
