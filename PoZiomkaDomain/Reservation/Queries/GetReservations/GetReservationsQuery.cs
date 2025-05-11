using MediatR;
using PoZiomkaDomain.Reservation.Dtos;

namespace PoZiomkaDomain.Reservation.Queries.GetReservations;

public record GetReservationsQuery : IRequest<IEnumerable<ReservationModel>>;
