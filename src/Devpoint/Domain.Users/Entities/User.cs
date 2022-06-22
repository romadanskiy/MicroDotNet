using System.ComponentModel.DataAnnotations;
using Domain.Developers.Entities;

namespace Domain.Users.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public Developer Developer { get; set; }
}