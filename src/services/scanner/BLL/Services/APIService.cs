using BLL.DTO;

namespace BLL.Services;

/// <summary>
/// Сервис для работы с API для получения информации о товаре
/// </summary>
public class ApiService
{
    /// <summary>
    /// Обратиться к API, чтобы получить информацию о товаре
    /// </summary>
    public APIGarbageInformationDTO GetGarbageInformationFromAPI(string barcode)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Сохранить данные, плученные от API  нашу БД
    /// </summary>
    public GarbageInformation SaveGarbageInformationToDataBase(APIGarbageInformationDTO garbageInformation)
    {
        throw new NotImplementedException();
    }
}