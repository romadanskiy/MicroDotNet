using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Payments.Replenishments;
using Services.Payments.Wallets;
using Web.API.Controllers.Payments.DTOs;

namespace Web.API.Controllers.Payments;

[ApiController]
[Route("replenishments")]
public class ReplenishmentController : Controller
{
    private readonly IReplenishmentService _replenishmentService;
    private readonly IWalletService _walletService;

    public ReplenishmentController(IReplenishmentService replenishmentService, IWalletService walletService)
    {
        _replenishmentService = replenishmentService;
        _walletService = walletService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllReplenishments()
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();
        
        var replenishments = await _replenishmentService.GetAllReplenishments()
            .Where(w => w.Wallet.Developer.Id == devId)
            .OrderByDescending(w => w.DateTime)
            .ToListAsync();
        var result = replenishments.Select(replenishment => new ReplenishmentDto(replenishment)).ToList();

        return Json(result);
    }

    [HttpGet]
    [Route("{replenishmentIds}")]
    public async Task<IActionResult> GetReplenishments(List<int> replenishmentIds)
    {
        var replenishments = await _replenishmentService.GetReplenishments(replenishmentIds);
        var result = replenishments.Select(replenishment => new ReplenishmentDto(replenishment));

        return Ok(result);
    }

    [HttpGet]
    [Route("{replenishmentId:int}")]
    public async Task<IActionResult> GetReplenishment(int replenishmentId)
    {
        var replenishment = await _replenishmentService.GetReplenishment(replenishmentId);
        var result = new ReplenishmentDto(replenishment);

        return Ok(result);
    }

    [HttpGet]
    [Route("{replenishmentId}/wallet")]
    public async Task<IActionResult> GetReplenishmentWallet(int replenishmentId)
    {
        var wallet = await _replenishmentService.GetReplenishmentWallet(replenishmentId);
        var result = new WalletDto(wallet);

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateReplenishment([FromBody] PaymentDto paymentDto)
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();
        
        var wallet = await _walletService.GetDeveloperWallet(devId.Value) ?? 
                     await _walletService.CreateWallet(devId.Value);

        var replenishment = await _replenishmentService.CreateReplenishment(paymentDto.Amount, wallet.Id);

        return Json(new ReplenishmentDto(replenishment));
    }
}