using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BLL.Configuration;
using BLL.DTO;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Scanner.Models;

namespace Scanner.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController: Controller
{
    public AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody]RegisterUserDto registerUserDto)
    {
        try
        {
            var user = new User(registerUserDto.Email, registerUserDto.Password, registerUserDto.PasswordRepeat);
            if (user.Validate())
            {
                await _accountService.RegisterUserAsync(user);
                return new ApiResult();
            }

            return new ApiResult(user.ValidationErrorMessages);
        }
        catch (ApplicationException ex)
        {
            return new ApiResult(ex);
        }
        catch
        {
            return new ApiResult(new List<string> { "Не удалось выполнить запрос!" });
        }
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> LoginAsync([FromBody]LoginDto loginDto)
    {
        try
        {
            var user = new User(loginDto.Email, loginDto.Password);
            if (user.Validate())
            {
                var data = await _accountService.LoginAsync(user);
                return new ApiResult(data);
            }

            return new ApiResult(user.ValidationErrorMessages);
        }
        catch (ApplicationException ex)
        {
            return new ApiResult(ex);
        }
        catch
        {
            return new ApiResult(new List<string> { "Не удалось выполнить запрос!" });
        }
    }
    
    [HttpPost]
    [Route("RefreshToken")]
    [Authorize]
    public async Task<IActionResult> RefreshTokenAsync()
    {
        try
        {
            var refreshToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var userId = HttpContext.User.GetUserId();

            var refreshData = new RefreshTokenData(userId, refreshToken);
            if (refreshData.Validate())
            {
                var loginData = await _accountService.RefreshTokenAsync(refreshData);
                return new ApiResult(loginData);
            }

            return new ApiResult(refreshData.ValidationErrorMessages);
        }
        catch (ApplicationException ex)
        {
            return new ApiResult(ex);
        }
        catch
        {
            return new ApiResult(new List<string> { "Не удалось выполнить запрос!" });
        }
    }
}