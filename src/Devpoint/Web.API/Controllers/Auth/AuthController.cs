using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Developers.Developers;
using Services.Payments.Wallets;
using Services.Users.Users;
using Web.API.Controllers.Auth.DTOs;
using Web.API.Controllers.Developers.DTOs;

namespace Web.API.Controllers.Auth;

[ApiController]
[Route("users")]
public class AuthController : Controller
{
    private readonly IDeveloperService _developerService;
    private readonly IUserService _userService;
    private readonly IWalletService _walletService;

    public AuthController(
        IDeveloperService developerService, 
        IUserService userService,
        IWalletService walletService)
    {
        _developerService = developerService;
        _userService = userService;
        _walletService = walletService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Unauthorized();

        var developer = await _userService.GetDeveloperByUser(userId.Value);
        if (developer == null)
            return StatusCode(500, "Something is wrong...");
        
        return Json(new DeveloperDto(developer));
    }
    
    [HttpGet("{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var user = await _userService.GetUserByEmail(email);
        if (user == null)
            return NotFound();
        
        var developer = await _userService.GetDeveloperByUser(user.Id);
        if (developer == null)
            return StatusCode(500, "Something is wrong...");
        
        return Json(new DeveloperDto(developer));
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        // TODO: Go to auth service
        var user = await _userService.GetUserByLogin(dto.Login);
        if (user is null)
            return NotFound("Login or password is incorrect.");

        var developer = await _userService.GetDeveloperByUser(user.Id);
        
        var identity = GetIdentity(dto.Login, user.Id, developer.Id);
        var token = GetToken(identity);
        
        return Ok(new
        {
            token,
            developer = new DeveloperDto(developer)
        });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // TODO: Go to auth service
        var user = await _userService.GetUserByLogin(dto.Login);
        if (user is not null)
            return BadRequest($"Login '{dto.Login}' already exists.");
        
        user = await _userService.GetUserByEmail(dto.Email);
        if (user is not null)
            return BadRequest($"Email '{dto.Email}' has already been taken.");

        var developer = await _developerService.CreateDeveloper(dto.Login, $"Registered on {DateTime.Now:dd.MM.yyyy}");
        var wallet = await _walletService.CreateWallet(developer.Id);
        var userId = await _userService.CreateUser(Guid.NewGuid(), dto.Login, dto.Email, developer.Id);
        
        var identity = GetIdentity(dto.Login, userId, developer.Id);
        var token = GetToken(identity);
        
        return Json(new
        {
            token,
            developer = new DeveloperDto(developer)
        });
    }

    private ClaimsIdentity GetIdentity(string login, Guid userId, Guid developerId)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, login),
            new(UserExtensions.DEV_ID_CLAIM, developerId.ToString()),
            new(UserExtensions.USER_ID_CLAIM, userId.ToString())
        };
        ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }

    private string GetToken(ClaimsIdentity identity)
    {
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}