using System.Security.Claims;

namespace PoZiomkaDomain.Common.Interface;

public class NotATokenException : Exception;
public class TokenExpiredException : Exception;
public class TokenValidationException : Exception;

public interface IJwtService
{
    public Task<string> GenerateToken(ClaimsIdentity identity, TimeSpan lifetime);

    public Task<ClaimsIdentity> ReadToken(string token);
}
