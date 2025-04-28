using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Communication;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Communication.Dtos;

namespace PoZiomkaApi.Controllers;

[Route("/communication")]
[ApiController]
public class CommunicationController(IMediator mediator) : Controller
{
    [HttpGet("get-student")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IEnumerable<CommunicationModel>> GetStudentCommunications()
    {
        return [
            new CommunicationModel(1, 1, "Testowe powiadomienie 1"),
            new CommunicationModel(2, 1, "Testowe powiadomienie 2"),
            new CommunicationModel(3, 1, "Testowe powiadomienie 3"),
            new CommunicationModel(4, 1, "Testowe powiadomienie 4"),
            new CommunicationModel(5, 1, "Testowe powiadomienie 5"),
        ];
    }

    [HttpPost("send")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> SendCommunication(CommunicationSendRequest sendRequest)
    {
        return NotFound();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> DeleteCommunication(int id)
    {
        return NotFound();
    }
}
