namespace PoZiomkaDomain.Student.Dtos;

public record StudentDisplay(int Id, string Email, string? FirstName, string? LastName, string? IndexNumber, string? PhoneNumber, int? ReservationId, bool? HasAcceptedReservation)
{
    public bool CanFillForms { get => FirstName != null && LastName != null && IndexNumber != null && PhoneNumber != null; }
}
