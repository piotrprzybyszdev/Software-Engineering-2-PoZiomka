using System.Security.Claims;

namespace PoZiomkaApi.Utils;
public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaims = user.FindFirst(ClaimTypes.NameIdentifier).Value;
        if (userIdClaims == null)
        {
            throw new Exception("User does not have a claim with name identifier");
        }
        return int.Parse(userIdClaims);
    }
}

