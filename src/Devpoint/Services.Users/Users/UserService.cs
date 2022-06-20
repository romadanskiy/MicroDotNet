using Data.Core;
using Domain.Developers.Entities;
using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Developers.Developers;

namespace Services.Users.Users;

public class UserService : IUserService
{
    private readonly ApplicationContext _context;
    private readonly IDeveloperService _developerService;

    public UserService(ApplicationContext context, IDeveloperService developerService)
    {
        _context = context;
        _developerService = developerService;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users;
    }

    public async Task<User?> GetUser(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);

        return user;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());

        return user;
    }
    
    public async Task<User?> GetUserByLogin(string login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Login.ToLower() == login.ToLower());

        return user;
    }
    
    public async Task<Developer?> GetDeveloperByUser(Guid userId)
    {
        var user = await GetUser(userId);
        if (user is null)
            return null;
        await _context.Entry(user).Reference(p => p.Developer).LoadAsync();

        return user.Developer;
    }
    
    public async Task<User?> GetUserByDeveloper(Guid developerId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Developer.Id == developerId);

        return user;
    }

    public async Task<Guid> CreateUser(Guid userId, string login, string email, Guid developerId)
    {
        var developer = await _developerService.GetDeveloper(developerId);
        var user = new User()
        {
            Login = login,
            Email = email,
            Id = userId,
            Developer = developer
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }
}