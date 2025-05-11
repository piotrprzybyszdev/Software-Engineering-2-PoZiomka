using MediatR;
using PoZiomkaDomain.Reservation.Dtos;

namespace PoZiomkaDomain.Reservation.Queries.GetReservations;

public class GetReservationsQueryHandler(IReservationRepository reservationRepository) : IRequestHandler<GetReservationsQuery, IEnumerable<ReservationModel>>
{
    public async Task<IEnumerable<ReservationModel>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
    {
        return await reservationRepository.GetAllReservations(cancellationToken);
    }
}