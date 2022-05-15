using System.Security.Claims;

namespace Scanner;

public static class ClaimsPrincipalExtension
{
    public static long? GetUserId(this ClaimsPrincipal identity)
    {
        var userIdString = identity.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        return string.IsNullOrEmpty(userIdString) ? null : Convert.ToInt64(userIdString);
    }
}