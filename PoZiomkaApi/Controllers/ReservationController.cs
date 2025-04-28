using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Reservation;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Reservation.Dtos;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaApi.Controllers;

[Route("/reservation")]
[ApiController]
public class ReservationController(IMediator mediator) : Controller
{
    [HttpGet("get")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IEnumerable<ReservationModel>> GetReservations()
    {
        return [
            new ReservationModel(1, 1, false),
            new ReservationModel(1, 2, true),
            new ReservationModel(1, 3, false),
            new ReservationModel(1, 4, true)
        ];
    }

    [HttpGet("get/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<ReservationDisplay> GetReservation(int id)
    {
        return new ReservationDisplay(id, new RoomModel(1, 3, 311, 2), [
            new StudentDisplay(1, "test@gmail.com", "Jan", "Kowalski", "123456", null, id, false, null, true, false),
            new StudentDisplay(2, "test2@gmail.com", "Paweł", "Nowak", "654321", "111222333", id, false, null, false, false)
        ], false);
    }

    [HttpPut("update")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> UpdateReservation(ReservationUpdateRequest updateRequest)
    {
        return NotFound();
    }
}
