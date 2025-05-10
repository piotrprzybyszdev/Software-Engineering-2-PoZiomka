using PoZiomkaDomain.Reservation.Commands.UpdateReservation;
using System.Security.Claims;

namespace PoZiomkaApi.Requests.Reservation;

public record ReservationUpdateRequest(int Id, bool IsAcceptation)
{
    public UpdateReservationCommand ToUpdateReservationCommand(ClaimsPrincipal user) => new(Id, IsAcceptation, user);
};
