using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Payments.Bills;
using Services.Payments.Earnings;
using Web.API.Controllers.Payments.DTOs;
using Web.API.Controllers.Subscriptions.DTOs;

namespace Web.API.Controllers.Payments;

[ApiController]
[Route("earnings")]
public class EarningController : Controller
{
    private readonly IEarningService _earningService;

    public EarningController(IEarningService earningService)
    {
        _earningService = earningService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllBills()
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();
        
        var earnings = await _earningService.GetAllEarnings()
            .Include(earning => earning.Tariff)
            .Include(earning => earning.WalletFrom.Developer)
            .Where(earning => earning.WalletTo.Developer.Id == devId)
            .OrderByDescending(earning => earning.DateTime)
            .ToListAsync();
        var result = earnings.Select(earning => new EarningDto(earning));

        return Json(result);
    }
}