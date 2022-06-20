using System.ComponentModel.DataAnnotations;

namespace Web.API.Controllers.Auth.DTOs;

public class RegisterDto
{
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}