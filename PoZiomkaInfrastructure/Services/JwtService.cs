using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PoZiomkaDomain.Common.Interface;
using System.Security.Claims;
using System.Text;

namespace PoZiomkaInfrastructure.Services;

public class JwtService(string key, string issuer, string audience) : IJwtService
{
    private readonly SigningCredentials credentials = new(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature
    );
    private readonly JsonWebTokenHandler handler = new();

    public Task<string> GenerateToken(ClaimsIdentity identity, TimeSpan lifetime)
    {
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = identity,
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.UtcNow.Add(lifetime),
            SigningCredentials = credentials
        };

        return Task.FromResult(handler.CreateToken(tokenDescriptor));
    }

    public async Task<ClaimsIdentity> ReadToken(string token)
    {
        JsonWebToken jwt;
        try
        {
            jwt = handler.ReadJsonWebToken(token);
        }
        catch (SecurityTokenMalformedException)
        {
            throw new NotATokenException();
        }

        var result = await handler.ValidateTokenAsync(jwt, new TokenValidationParameters()
        {
            IssuerSigningKey = credentials.Key,
            ValidIssuer = issuer,
            ValidAudience = audience,
            ClockSkew = TimeSpan.Zero
        });

        if (result.IsValid)
            return new ClaimsIdentity(jwt.Claims);

        throw result.Exception switch
        {
            SecurityTokenExpiredException => new TokenExpiredException(),
            SecurityTokenValidationException => new TokenValidationException(),
            _ => result.Exception,
        };
    }
}
