using Microsoft.EntityFrameworkCore;

namespace DAL.Entity;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public long Id { get; set; }
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string? RefreshToken { get; set; }
}