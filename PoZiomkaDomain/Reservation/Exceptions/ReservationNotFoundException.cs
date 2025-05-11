using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Reservation.Exceptions;

public class ReservationNotFoundException : DomainException
{
    public ReservationNotFoundException(string message) : base(message)
    {
    }
    public ReservationNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
