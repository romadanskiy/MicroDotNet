using System.Security.Claims;

namespace Web.API;

public static class UserExtensions
{
    public const string DEV_ID_CLAIM = "developerId";
    public const string USER_ID_CLAIM = "userId";

    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var guid = user.FindFirst(USER_ID_CLAIM)?.Value;
        if (guid == null)
            return null;
        return new Guid(guid);
    }
    
    public static Guid? GetDevId(this ClaimsPrincipal user)
    {
        var guid = user.FindFirst(DEV_ID_CLAIM)?.Value;
        if (guid == null)
            return null;
        return new Guid(guid);
    }
}