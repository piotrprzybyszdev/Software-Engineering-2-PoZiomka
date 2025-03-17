namespace PoZiomkaDomain.Student.Dtos;


public record StudentModel(int Id, string Email, string? FirstName, string? LastName, string PasswordHash,
    bool IsConfirmed, string? PhoneNumber, string? IndexNumber,
    int? ReservationId, bool? HasAcceptedReservation, bool IsPhoneNumberHidden, bool IsIndexNumberHidden)
{

    public StudentDisplay ToStudentDisplay(bool hidePersonalInfo) => new(
        Id, Email, FirstName, LastName,
        hidePersonalInfo && IsIndexNumberHidden ? null : IndexNumber,
        hidePersonalInfo && IsPhoneNumberHidden ? null : PhoneNumber,
        ReservationId, HasAcceptedReservation
    );
};
