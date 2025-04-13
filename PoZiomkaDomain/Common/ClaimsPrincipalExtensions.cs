using System.Security.Claims;

namespace PoZiomkaDomain.Common;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaims = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaims == null)
            return null;
        return int.Parse(userIdClaims);
    }
}
