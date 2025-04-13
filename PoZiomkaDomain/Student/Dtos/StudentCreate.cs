namespace PoZiomkaDomain.Student.Dtos;

public record StudentCreate(string Email, string? FirstName, string? LastName, string? IndexNumber, string? PhoneNumber);
