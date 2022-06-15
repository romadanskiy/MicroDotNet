using System.Security.Claims;

namespace RuOverflow.Questions.Extensions;

public static class ClaimExtensions
{
    public static IEnumerable<Claim> GetClaims(this IHttpContextAccessor accessor)
    {
        var userClaims = accessor.HttpContext?.User.Claims;
        if (userClaims is null)
        {
            throw new ApplicationException("User has not claims");
        }
        return userClaims;
    }
    
    public static Guid GetUserId(this IHttpContextAccessor accessor)
    {
        var claims = GetClaims(accessor);
        return new Guid(claims.First(x=> x.Type == ClaimTypes.NameIdentifier).Value);
    }
    
    public static string GetUserName(this IHttpContextAccessor accessor)
    {
        var claims = GetClaims(accessor);
        return claims.First(x=> x.Type == ClaimTypes.Name).Value;
    }
    
    public static string GetUserEmail(this IHttpContextAccessor accessor)
    {
        var claims = GetClaims(accessor);
        return claims.First(x=> x.Type == ClaimTypes.Email).Value;
    }
}