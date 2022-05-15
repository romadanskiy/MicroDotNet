namespace BLL.Models;

public class RefreshTokenData: Validateable
{
    private long _id;
    private string _refreshToken;

    public long Id => _id;
    public string RefreshToken => _refreshToken;

    public RefreshTokenData(long? id, string refreshToken)
    {
        _id = id ?? 0;
        _refreshToken = refreshToken;
    }
    

    public override bool Validate()
    {
        var errorMessages = new List<string>();

        if (Id == 0)
        {
            errorMessages.Add("Не удалось получить информацию о пользователе, выполнившем запрос!");
        }
        
        if (string.IsNullOrEmpty(RefreshToken))
        {
            errorMessages.Add("Не удалось получить токен обновления из заголовка!");
        }

        if (errorMessages.Any())
        {
            ValidationErrorMessages.AddRange(errorMessages);
            return false;
        }
        return true;
    }
}