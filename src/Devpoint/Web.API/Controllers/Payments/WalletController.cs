using Microsoft.AspNetCore.Mvc;
using Services.Payments.Wallets;
using Web.API.Controllers.Developers.DTOs;
using Web.API.Controllers.Payments.DTOs;

namespace Web.API.Controllers.Payments;

[ApiController]
[Route("wallets")]
public class WalletController : Controller
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllWallets()
    {
        var wallets = await _walletService.GetAllWallets();
        var result = wallets.Select(wallet => new WalletDto(wallet));

        return Ok(result);
    }

    [HttpGet]
    [Route("{walletIds}")]
    public async Task<IActionResult> GetWallets(List<int> walletIds)
    {
        var wallets = await _walletService.GetWallets(walletIds);
        var result = wallets.Select(wallet => new WalletDto(wallet));

        return Ok(result);
    }
    
    
    [HttpGet]
    [Route("{walletId:int}")]
    public async Task<IActionResult> GetWallet(int walletId)
    {
        var wallet = await _walletService.GetWallet(walletId);
        var result = new WalletDto(wallet);

        return Ok(result);
    }
    
    [HttpGet]
    [Route("user")]
    public async Task<IActionResult> GetUserWallet()
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();
        
        var wallet = await _walletService.GetDeveloperWallet(devId.Value) ?? 
                     await _walletService.CreateWallet(devId.Value);

        return Json(wallet);
    }

    [HttpGet]
    [Route("{walletId:int}/developer")]
    public async Task<IActionResult> GetWalletDeveloper(int walletId)
    {
        var developer = await _walletService.GetWalletDeveloper(walletId);
        var result = new DeveloperDto(developer);

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateWallet(Guid developerId)
    {
        var walletId = await _walletService.CreateWallet(developerId);

        return Ok(walletId);
    }

    [HttpPut]
    [Route("{walletId:int}/update/amount")]
    public async Task<IActionResult> UpdateAmount(int walletId, int amount)
    {
        await _walletService.UpdateAmount(walletId, amount);

        return Ok();
    }
}