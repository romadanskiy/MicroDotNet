using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Web.API;

public class AuthOptions
{
    public const string ISSUER = "DebpointWebApi";
    public const string AUDIENCE = "DevpointAngular";
    const string KEY = "devpoint_supersecretkey";
    public const int LIFETIME = 60;
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}