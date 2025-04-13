namespace PoZiomkaDomain.Student.Dtos;

public record StudentUpdate(int Id, string? FirstName, string? LastName, string? PhoneNumber, string? IndexNumber, bool IsPhoneNumberHidden, bool IsIndexNumberHidden);
