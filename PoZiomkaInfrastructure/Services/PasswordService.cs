using PoZiomkaDomain.Common.Interface;

namespace PoZiomkaInfrastructure.Services;

public class PasswordService : IPasswordService
{
    public string ComputeHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyHash(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
