using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Configuration;

public class TokenOptions
{
    public const string ISSUER = "ScannerRestApiServer";
    public const string AUDIENCE = "ScannerMobileApp";
    const string KEY = "ahahahahahahhahahahahahahahaah";
    public const int LIFETIME = 5;
    public const int REFRESH_LIFETIME = 60*24*30;
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}