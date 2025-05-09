using PoZiomkaDomain.Reservation.Commands.UpdateReservation;
using System.Security.Claims;

namespace PoZiomkaApi.Requests.Reservation;

public record ReservationUpdateRequest(int Id, bool IsAcceptation, ClaimsPrincipal User)
{
    public UpdateReservationCommand ToUpdateReservationCommand() => new(Id, IsAcceptation, User);
};
