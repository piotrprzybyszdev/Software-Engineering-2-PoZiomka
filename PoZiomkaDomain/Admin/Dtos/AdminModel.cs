namespace PoZiomkaDomain.Admin.Dtos;

public record AdminModel(int Id, string Email, string PasswordHash)
{
    public AdminDisplay ToAdminDisplay() => new(Id, Email);
}
