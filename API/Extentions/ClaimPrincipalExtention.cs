using System.Security.Claims;
using API.Exceptions;
using Common;

namespace API.Extentions;

public static class ClaimPrincipalExtention
{
    public static T? GetClaimValue<T>(this ClaimsPrincipal user, string claim, bool isRequired = true)
    {
        string? claimValue = user.Claims.FirstOrDefault(x => x.Type == claim)?.Value;

        if (claimValue == default)
            return isRequired ? throw new AuthException($"{claim} from JWT not found") : default;

        return claimValue.Convert<T>();
    }
}