using MediatR;
using PoZiomkaDomain.Reservation.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Reservation.Queries.GetReservation;

public record GetReservationQuery(int Id, ClaimsPrincipal User) : IRequest<ReservationDisplay>;
