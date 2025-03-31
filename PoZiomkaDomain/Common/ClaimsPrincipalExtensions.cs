using PoZiomkaDomain.Exceptions;
using System.Security.Claims;

namespace PoZiomkaDomain.Common;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaims = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedException("User does not have a claim with name identifier");
        return int.Parse(userIdClaims);
    }
}

