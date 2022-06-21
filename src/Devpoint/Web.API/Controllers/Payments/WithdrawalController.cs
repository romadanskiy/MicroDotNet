using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Payments.Wallets;
using Services.Payments.Withdrawals;
using Web.API.Controllers.Payments.DTOs;

namespace Web.API.Controllers.Payments;

[ApiController]
[Route("withdrawals")]
public class WithdrawalController : Controller
{
    private readonly IWithdrawalService _withdrawalService;
    private readonly IWalletService _walletService;

    public WithdrawalController(IWithdrawalService withdrawalService, IWalletService walletService)
    {
        _withdrawalService = withdrawalService;
        _walletService = walletService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllWithdrawals()
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();
        
        var withdrawals = await _withdrawalService.GetAllWithdrawals()
            .Where(w => w.Wallet.Developer.Id == devId)
            .OrderByDescending(w => w.DateTime)
            .ToListAsync();
        var result = withdrawals.Select(withdrawal => new WithdrawalDto(withdrawal)).ToList();

        return Json(result);
    }

    [HttpGet]
    [Route("withdrawalIds")]
    public async Task<IActionResult> GetWithdrawals(List<int> withdrawalIds)
    {
        var withdrawals = await _withdrawalService.GetWithdrawals(withdrawalIds);
        var result = withdrawals.Select(withdrawal => new WithdrawalDto(withdrawal));

        return Ok(result);
    }

    [HttpGet]
    [Route("{withdrawalId:int}")]
    public async Task<IActionResult> GetWithdrawal(int withdrawalId)
    {
        var withdrawal = await _withdrawalService.GetWithdrawal(withdrawalId);
        var result = new WithdrawalDto(withdrawal);

        return Ok(result);
    }

    [HttpGet]
    [Route("{withdrawalId:int}/wallet")]
    public async Task<IActionResult> GetWithdrawalWallet(int withdrawalId)
    {
        var wallet = await _withdrawalService.GetWithdrawalWallet(withdrawalId);
        var result = new WalletDto(wallet);

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateWithdrawal([FromBody] PaymentDto paymentDto)
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();
        
        var wallet = await _walletService.GetDeveloperWallet(devId.Value) ?? 
                     await _walletService.CreateWallet(devId.Value);
         
        var withdrawal = await _withdrawalService.CreateWithdrawal(paymentDto.Amount, wallet.Id);

        return Json(new WithdrawalDto(withdrawal));
    }
}