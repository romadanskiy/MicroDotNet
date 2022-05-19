namespace BLL.Models.Types;

public class Password: Validateable
{
    private string _password;

    public Password(string password)
    {
        _password = password;
    }

    public override bool Validate()
    {
        if (string.IsNullOrEmpty(_password))
        {
            ValidationErrorMessages.Add("Не указан пароль!");
        }
        
        else if (_password.Length<8)
        {
            ValidationErrorMessages.Add("Минимальная длина пароля: 8 символов!");
        }

        return !ValidationErrorMessages.Any();
    }

    public string GetValue()
    {
        return _password;
    }
}