using System.Security.Claims;

namespace PoZiomkaDomain.Common;

public interface IJwtService
{
    public Task<string> GenerateToken(ClaimsIdentity identity, TimeSpan lifetime);

    public Task<ClaimsIdentity> ReadToken(string token);
}
