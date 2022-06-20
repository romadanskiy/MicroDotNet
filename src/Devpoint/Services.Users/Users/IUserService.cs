using Domain.Developers.Entities;
using Domain.Users.Entities;

namespace Services.Users.Users;

public interface IUserService
{
    public Task<List<User>> GetAllUsers();

    public Task<User?> GetUser(Guid userId);
    
    public Task<User?> GetUserByEmail(string email);
    
    public Task<User?> GetUserByLogin(string email);

    public Task<User?> GetUserByDeveloper(Guid developerId);
    
    public Task<Developer?> GetDeveloperByUser(Guid userId);

    public Task<Guid> CreateUser(Guid userId, string login, string email, Guid developerId);
}