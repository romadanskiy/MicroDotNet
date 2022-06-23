using BLL.DTO;
using BLL.Models.Types;

namespace BLL.Models;

public class ReceptionPoint: Validateable
{
    public long Id { get; set; }
    public string Name { get; }
    public string? Description { get; }
    public Address Address { get; }
    public List<GarbageType> GarbageTypes { get; set; }

    public ReceptionPoint(string name, string description, Address address, List<GarbageType> garbageTypes, long id = 0)
    {
        Id = id;
        Name = name;
        Description = description;
        Address = address;
        GarbageTypes = garbageTypes;
    }
    
    public override bool Validate()
    {
        var errorMessages = new List<string>();

        if (string.IsNullOrEmpty(Name))
        {
            errorMessages.Add("Не указано название пункта утилизации!");
        }
        
        if (!Address.Validate())
        {
            errorMessages.AddRange(Address.ValidationErrorMessages);
        }
        
        if (GarbageTypes == null || !GarbageTypes.Any())
        {
            errorMessages.Add("У пункта приема должна быть указана хотябы одна категория принимаемых отходов!");
        }

        if (errorMessages.Any())
        {
            ValidationErrorMessages.AddRange(errorMessages);
            return false;
        }
        return true;
    }
}