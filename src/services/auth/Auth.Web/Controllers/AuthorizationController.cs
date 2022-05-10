using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationServer.Web.Domain;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants.Permissions;
using static OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreConstants;

namespace AuthorizationServer.Web.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthorizationController(
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request?.IsPasswordGrantType() == true)
            {
                /*var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                        [Properties.ErrorDescription] =
                            "The username/password couple is invalid."
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                // Validate the username/password parameters and ensure the account is not locked out.
                var result =
                    await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                        [Properties.ErrorDescription] =
                            "The username/password couple is invalid."
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                // Create a new ClaimsPrincipal containing the claims that
                // will be used to create an id_token, a token or a code.
                var principal = await _signInManager.CreateUserPrincipalAsync(user);

                // Set the list of scopes granted to the client application.
                principal.SetScopes(new[]
                {
                    Scopes.Email,
                    Scopes.Profile,
                    Scopes.Roles
                }.Intersect(request.GetScopes()));

                foreach (var claim in principal.Claims)
                {
                    claim.SetDestinations(GetDestinations(claim, principal));
                }*/
                // Validate the user credentials.
                // Note: to mitigate brute force attacks, you SHOULD strongly consider
                // applying a key derivation function like PBKDF2 to slow down
                // the password validation process. You SHOULD also consider
                // using a time-constant comparer to prevent timing attacks.
                if (request.Username != "alice@wonderland.com" ||
                    request.Password != "P@ssw0rd")
                {
                    return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }
                // Create a new ClaimsIdentity holding the user identity.
                var identity = new ClaimsIdentity(
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    OpenIddictConstants.Claims.Name,
                    OpenIddictConstants.Claims.Role);
                // Add a "sub" claim containing the user identifier, and attach
                // the "access_token" destination to allow OpenIddict to store it
                // in the access token, so it can be retrieved from your controllers.
                identity.AddClaim(OpenIddictConstants.Claims.Subject,
                    "71346D62-9BA5-4B6D-9ECA-755574D628D8",
                    OpenIddictConstants.Destinations.AccessToken);
                identity.AddClaim(OpenIddictConstants.Claims.Name, "Alice",
                    OpenIddictConstants.Destinations.AccessToken);
                // ... add other claims, if necessary.
                var principal = new ClaimsPrincipal(identity);
                // Ask OpenIddict to generate a new token and return an OAuth2 token response.
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            throw new NotImplementedException("The specified grant type is not implemented.");
        }
        
        private IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch (claim.Type)
            {
                case OpenIddictConstants.Claims.Name:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Profile))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                case OpenIddictConstants.Claims.Email:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Email))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                case OpenIddictConstants.Claims.Role:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Roles))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return OpenIddictConstants.Destinations.AccessToken;
                    yield break;
            }
        }
    }
}