using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Room;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaApi.Controllers;

[Route("/room")]
[ApiController]
public class RoomController(IMediator mediator) : ControllerBase
{
    [HttpGet("get")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Get()
    {
        return Ok(new List<RoomDisplay>([new RoomDisplay(
            1, 3, 311, 5, [
                new StudentDisplay(1, "test@gmail.com", "Jan", "Kowalski", "333444", "777888999", null, null, 1, false, false)
            ]
        )]));
    }

    [HttpGet("get/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(new RoomDisplay(
            1, 3, 311, 5, [
                new StudentDisplay(1, "test@gmail.com", "Jan", "Kowalski", "333444", "777888999", null, null, 1, false, false)
            ]
        ));
    }

    [HttpPost("create")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Create([FromBody] CreateRequest createRequest)
    {
        return NotFound();
    }

    [HttpPut("update")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update([FromBody] UpdateRequest updateRequest)
    {
        return NotFound();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return NotFound();
    }
}
