using System.Text;
using BLL.Models;
using BLL.Models.Types;

namespace BLL.Models;

public class User: Validateable
{
    private Email _email;
    private Password _password;
    private Password? _passwordRepeat;

    public Email Email => _email;
    public Password Password => _password;

    public User(string email, string password)
    {
        _email = new Email(email);
        _password = new Password(password);
    }
    public User(string email, string password, string passwordRepeat)
    {
        _email = new Email(email);
        _password = new Password(password);
        _passwordRepeat = new Password(passwordRepeat);
    }

    public override bool Validate()
    {
        _email.Validate();
        _password.Validate();
        var errorMessages = new List<string>();


        errorMessages.AddRange(_email.ValidationErrorMessages);
        errorMessages.AddRange(_password.ValidationErrorMessages);

        if (_passwordRepeat != null && _password.GetValue() != _passwordRepeat.GetValue())
        {
            errorMessages.Add("Пароли не совпадают!");
        }

        if (errorMessages.Any())
        {
            ValidationErrorMessages.AddRange(errorMessages);
            return false;
        }
        return true;
    }
}