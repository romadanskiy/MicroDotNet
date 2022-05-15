namespace BLL.Models.Types;

public class Email: Validateable
{
    private string _email;

    public Email(string email)
    {
        _email = email;
    }

    public override bool Validate()
    {
        if (string.IsNullOrEmpty(_email))
        {
            ValidationErrorMessages.Add("Не указан адрес электронной почты!");
        }
        
        else if (!_email.Contains("@") || !_email.Split("@").Last().Contains("."))
        {
            ValidationErrorMessages.Add("Адрес электронной почты имеет неверный формат!");
        }

        return !ValidationErrorMessages.Any();
    }

    public string GetValue()
    {
        return _email;
    }
}