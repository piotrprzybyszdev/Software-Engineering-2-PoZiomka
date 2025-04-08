using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Room;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Room.Dtos;

namespace PoZiomkaApi.Controllers;

[Route("/room")]
[ApiController]
public class RoomController(IMediator mediator) : ControllerBase
{
    [HttpGet("get")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Get()
    {
        return Ok(new List<RoomDisplay>([new RoomDisplay(1, 3, 311, 5, null, [1])]));
    }

    [HttpGet("get/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(new RoomDisplay(1, 3, 311, 5, null, [1]));
    }

    [HttpPost("create")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Create([FromBody] CreateRequest createRequest)
    {
        await mediator.Send(createRequest.ToCreateRoomCommand());
        return Ok();
    }

    [HttpPut("add-student")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> AddStudent([FromBody] AddStudentRequest addStudentRequest)
    {
        return NotFound();
    }

    [HttpPut("remove-student")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> RemoveStudent([FromBody] RemoveStudentRequest removeStudentRequest)
    {
        return NotFound();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return NotFound();
    }
}
