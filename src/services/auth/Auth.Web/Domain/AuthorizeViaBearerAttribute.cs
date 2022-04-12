using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace AuthorizationServer.Web.Domain
{
    public class AuthorizeViaBearerAttribute : AuthorizeAttribute
    {
        public AuthorizeViaBearerAttribute()
        {
            AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        }
    }
}
