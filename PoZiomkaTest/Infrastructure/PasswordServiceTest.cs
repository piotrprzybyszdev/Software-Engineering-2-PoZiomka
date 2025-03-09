using PoZiomkaInfrastructure.Services;

namespace PoZiomkaTest.Infrastructure;

public class PasswordServiceTest
{
    [Fact]
    public void SamePasswordPasses()
    {
        PasswordService passwordService = new();
        var password = "test123";

        var hash = passwordService.ComputeHash(password);
        Assert.True(passwordService.VerifyHash(password, hash));
    }

    [Fact]
    public void DifferentPasswordFails()
    {
        PasswordService passwordService = new();
        var password1 = "test123";
        var password2 = "test1234";

        var hash = passwordService.ComputeHash(password1);
        Assert.False(passwordService.VerifyHash(password2, hash));
    }
}
