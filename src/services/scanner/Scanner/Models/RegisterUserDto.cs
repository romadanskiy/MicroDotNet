namespace Scanner.Models;

public class RegisterUserDto
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string PasswordRepeat { get; set; }
}