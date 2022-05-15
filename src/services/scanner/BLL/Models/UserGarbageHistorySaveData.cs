namespace BLL.Models;

public class UserGarbageHistorySaveData:Validateable
{
    public string Barcode { get; }
    public long? UserId { get; }

    public UserGarbageHistorySaveData(string barcode, long? userId)
    {
        Barcode = barcode;
        UserId = userId;
    }

    public override bool Validate()
    {
        var errorMessages = new List<string>();

        if (string.IsNullOrEmpty(Barcode))
        {
            errorMessages.Add("Не указан штрихкод товара!");
        }
        
        if (!UserId.HasValue)
        {
            errorMessages.Add("Не удалось получить данные пользователя, выполнившего запрос!");
        }

        if (errorMessages.Any())
        {
            ValidationErrorMessages.AddRange(errorMessages);
            return false;
        }
        return true;
    }
}