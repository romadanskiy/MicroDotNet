using System.Text.Json.Serialization;

namespace BLL.Models;

public class Validateable
{
    private List<string> validationErrorMessages;

    [JsonIgnore]
    public List<string> ValidationErrorMessages
    {
        get
        {
            if (validationErrorMessages == null)
            {
                validationErrorMessages = new List<string>();
            }

            return validationErrorMessages;
        }
    }

    public virtual bool Validate()
    {
        return true;
    }
}