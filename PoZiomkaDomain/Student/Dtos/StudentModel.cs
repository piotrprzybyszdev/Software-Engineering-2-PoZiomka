namespace PoZiomkaDomain.Student.Dtos;

public record StudentModel(int Id, string Email, string PasswordHash, bool IsConfirmed);
