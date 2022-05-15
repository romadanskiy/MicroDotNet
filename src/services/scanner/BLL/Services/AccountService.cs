using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BLL.Configuration;
using BLL.Models;
using BLL.Models.Helpers;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services;

public class AccountService
{
    private readonly ScannerDbContext _dbContext;
    private readonly PasswordHelper _passwordHelper;

    public AccountService(ScannerDbContext dbContext, PasswordHelper passwordHelper)
    {
        _dbContext = dbContext;
        _passwordHelper = passwordHelper;
    }
    
    public async Task RegisterUserAsync(User user)
    {
        if ( !await IsEmailAlreadyExist(user.Email.GetValue()))
        {
            var newDbUser = new DAL.Entity.User()
            {
                Email = user.Email.GetValue(),
                Password = _passwordHelper.HashPassword(user.Password.GetValue())
            };

            _dbContext.User.Add(newDbUser);
            await _dbContext.SaveChangesAsync();
            
            return;
        }

        throw new ApplicationException("Данный email уже занят!");
    }

    public async Task<LoginData> LoginAsync(User user)
    {
        var dbUser = await _dbContext.User.Where(x=>x.Email == user.Email.GetValue()).FirstOrDefaultAsync();

        if (dbUser == null)
        {
            throw new ApplicationException("Не найден пользователь с таким email!");
        }

        var hashedPassword = dbUser.Password;

        if (_passwordHelper.VerifyPassword(hashedPassword, user.Password.GetValue()))
        {
            var loginData = GetUserLoginData(dbUser);
            await SaveRefreshTokenToUserAsync(dbUser, loginData.RefreshToken);
            return loginData;
        }

        throw new ApplicationException("Указан неверный пароль!");
    }

    public async Task<LoginData> RefreshTokenAsync(RefreshTokenData refreshTokenData)
    {
        var user = await _dbContext.User.Where(x => x.Id == refreshTokenData.Id).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new ApplicationException("Не обнаружен пользователь с указанным идентифкатором!");
        }

        if (user.RefreshToken == null || user.RefreshToken != refreshTokenData.RefreshToken)
        {
            throw new ApplicationException("Невозможно обновить токен доступа: сохраненный токен обновления не совпадает с указанным в заголовке!");
        }
        
        var loginData = GetUserLoginData(user);
        await SaveRefreshTokenToUserAsync(user,  loginData.RefreshToken);

        return loginData;
    }

    private async Task SaveRefreshTokenToUserAsync(DAL.Entity.User user, string refreshToken)
    {
        user.RefreshToken = refreshToken;
        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> IsEmailAlreadyExist(string email)
    {
        return await _dbContext.User.Where(x => x.Email == email).AnyAsync();
    }
    
    private LoginData GetUserLoginData(DAL.Entity.User user)
    {
        var loginData = new LoginData(user.Id, GetAccessToken(user), GetRefreshToken(user));

        if (loginData == null || string.IsNullOrEmpty(loginData.AccessToken) ||
            string.IsNullOrEmpty(loginData.RefreshToken) || loginData.UserId == 0)
        {
            throw new ApplicationException("Не удалось сформировать данные для входа!");
        }

        return loginData;
    }

    private string GetAccessToken(DAL.Entity.User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultRoleClaimType, "AuthorizedUser"),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var now = DateTime.UtcNow;
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: TokenOptions.ISSUER,
            audience: TokenOptions.AUDIENCE,
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromMinutes(TokenOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(TokenOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        return  new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    private string GetRefreshToken(DAL.Entity.User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var now = DateTime.UtcNow;
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: TokenOptions.ISSUER,
            audience: TokenOptions.AUDIENCE,
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromMinutes(TokenOptions.REFRESH_LIFETIME)),
            signingCredentials: new SigningCredentials(TokenOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        return  new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}