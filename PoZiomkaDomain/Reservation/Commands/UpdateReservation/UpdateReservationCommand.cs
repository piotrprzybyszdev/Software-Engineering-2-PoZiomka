using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Reservation.Commands.UpdateReservation;

public record UpdateReservationCommand(int Id, bool IsAcceptation, ClaimsPrincipal User) : IRequest;
