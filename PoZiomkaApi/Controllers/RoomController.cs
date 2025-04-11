using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Room;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Room.Commands.DeleteRoom;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room.Queries.GetAllRooms;
using PoZiomkaDomain.Room.Queries.GetRoom;

namespace PoZiomkaApi.Controllers;

[Route("/room")]
[ApiController]
public class RoomController(IMediator mediator) : ControllerBase
{
    [HttpGet("get")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Get()
    {
        var command = new GetAllRoomsQuery();
        return Ok(await mediator.Send(command));
    }

    [HttpGet("get/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> GetById(int id)
    {
        var getRoom = new GetRoomQuery(id, User);
        var room = await mediator.Send(getRoom);

        return Ok(room);
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
        await mediator.Send(addStudentRequest.ToAddStudentCommand());
        return Ok();
    }

    [HttpPut("remove-student")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> RemoveStudent([FromBody] RemoveStudentRequest removeStudentRequest)
    {
        return NotFound();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteRoomCommand(id);
        await mediator.Send(command);

        return Ok();
    }
}
