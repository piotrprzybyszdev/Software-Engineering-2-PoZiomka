namespace PoZiomkaDomain.Student.Dtos;

public record StudentCreate(string Email, string PasswordHash, bool IsConfirmed);
