using PoZiomkaDomain.Common.Interface;
using System.Security.Claims;

namespace PoZiomkaInfrastructure.Services;

public class EmailContentGenerator(string appUrl, string confirmEmailPath, string passwordResetPath, IJwtService jwtService) : IEmailContentGenerator
{
    public string ConfirmationEmailSubject => "PoZiomka - Potwierdzenie adresu email";

    public string PasswordResetEmailSubject => "PoZiomka - Reset hasła";

    public async Task<string> GenerateEmailConfirmationEmail(string email)
    {
        return @$"
<h2>Kliknij poniższy przycisk aby potwierdzić adres email:<h2>
<a href={await GenerateAppCallback(confirmEmailPath, email)}>
    <button>Potwierdź email</button>
</a>";
    }

    public async Task<string> GeneratePasswordResetEmail(string email)
    {
        return @$"
<h2>Kliknij poniższy przycisk aby zresetować hasło:<h2>
<a href={await GenerateAppCallback(passwordResetPath, email)}>
    <button>Resetuj hasło</button>
</a>
";
    }

    private async Task<string> GenerateAppCallback(string path, string email)
    {
        var token = await jwtService.GenerateToken(new ClaimsIdentity([
            new Claim(ClaimTypes.Email, email)
        ]), TimeSpan.FromMinutes(20));

        return $"{appUrl}{path}/{token}";
    }
}
