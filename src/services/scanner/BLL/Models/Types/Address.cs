namespace BLL.Models.Types;

public class Address: Validateable
{
    public string FullAddress { get; }
    public string Longitude { get; }
    public string Latitude { get; }

    public Address(string fullAddress, string longitude, string latitude)
    {
        FullAddress = fullAddress;
        Longitude = longitude;
        Latitude = latitude;
    }

    public override bool Validate()
    {
        if (string.IsNullOrEmpty(FullAddress))
        {
            ValidationErrorMessages.Add("Не указан адрес!");
        }
        
        if (string.IsNullOrEmpty(Longitude))
        {
            ValidationErrorMessages.Add("Не указана долгота!");
        }
        
        if (string.IsNullOrEmpty(Latitude))
        {
            ValidationErrorMessages.Add("Не указана широта!");
        }

        return !ValidationErrorMessages.Any();
    }
}