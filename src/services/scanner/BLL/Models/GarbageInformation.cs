using BLL.Models;
using BLL.Models.Helpers;
using Microsoft.AspNetCore.Http;

namespace BLL.DTO;

/// <summary>
/// ДТО с информацией об отходах
/// </summary>
public class GarbageInformation: Validateable
{
    private string _imagePath;
    private string _name;
    private string _description;
    private string _barcode;
    private IEnumerable<GarbageType> _garbageTypes;

    public string ImagePath => _imagePath;
    public string Name => _name;
    public string Description => _description;
    public string Barcode => _barcode;
    public IEnumerable<GarbageType> GarbageTypes => _garbageTypes;

    public GarbageInformation(string imagePath, string name, string description, string barcode, IEnumerable<long> garbageTypeIds)
    {
        _imagePath = imagePath;
        _name = name;
        _description = description;
        _barcode = barcode;
        if (garbageTypeIds != null)
        {
            _garbageTypes = garbageTypeIds.Select(x=> new GarbageType(x));
        }
    }
    
    public GarbageInformation(string imagePath, string name, string description, string barcode, IEnumerable<DAL.Entity.GarbageType> garbageTypeIds)
    {
        _imagePath = imagePath;
        _name = name;
        _description = description;
        _barcode = barcode;
        if (garbageTypeIds != null)
        {
            _garbageTypes = garbageTypeIds.Select(x=> new GarbageType(x.Id,x.Name));
        }
    }

    public override bool Validate()
    {
        var errorMessages = new List<string>();

        if (string.IsNullOrEmpty(_name))
        {
            errorMessages.Add("Имя не может быть пустым!");
        }
        
        if (string.IsNullOrEmpty(_barcode))
        {
            errorMessages.Add("Штрихкод не может быть пустым!");
        }
        
        if (_garbageTypes == null || !_garbageTypes.Any())
        {
            errorMessages.Add("Должна быть указана как минимум одна категория отходов!");
        }

        if (errorMessages.Any())
        {
            ValidationErrorMessages.AddRange(errorMessages);
            return false;
        }
        return true;
    }
}