using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Communication;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Communication.Commands.DeleteCommunication;
using PoZiomkaDomain.Communication.Dtos;
using PoZiomkaDomain.Communication.Queries.GetStudentCommunications;

namespace PoZiomkaApi.Controllers;

[Route("/communication")]
[ApiController]
public class CommunicationController(IMediator mediator) : Controller
{
    [HttpGet("get-student")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IEnumerable<CommunicationDisplay>> GetStudentCommunications()
    {
        return await mediator.Send(new GetStudentCommunicationsQuery(User));
    }

    [HttpPost("send")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> SendCommunication(CommunicationSendRequest sendRequest)
    {
        await mediator.Send(sendRequest.ToSendCommunicationCommand());
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> DeleteCommunication(int id)
    {
        await mediator.Send(new DeleteCommunicationCommand(id, User));
        return Ok();
    }
}
