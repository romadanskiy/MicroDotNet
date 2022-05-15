namespace BLL.Models;

public class LoginData
{
    public LoginData(long id, string accessToken, string refreshToken )
    {
        UserId = id;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
    
    public long UserId { get; private set; }
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }
}