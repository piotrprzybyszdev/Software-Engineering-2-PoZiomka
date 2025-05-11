using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Reservation;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Reservation.Commands.UpdateReservation;
using PoZiomkaDomain.Reservation.Dtos;
using PoZiomkaDomain.Reservation.Queries.GetReservation;
using PoZiomkaDomain.Reservation.Queries.GetReservations;
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
        return await mediator.Send(new GetReservationsQuery());
    }

    [HttpGet("get/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<ReservationDisplay> GetReservation(int id)
    {
        return await mediator.Send(new GetReservationQuery(id, User));
    }

    [HttpPut("update")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IActionResult> UpdateReservation(ReservationUpdateRequest updateRequest)
    {
        await mediator.Send(updateRequest.ToUpdateReservationCommand(User));
        return Ok();
    }
}
