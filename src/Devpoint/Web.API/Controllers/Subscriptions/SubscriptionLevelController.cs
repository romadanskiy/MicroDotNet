using Microsoft.AspNetCore.Mvc;
using Services.Subscriptions.SubscriptionLevels;
using Web.API.Controllers.Subscriptions.DTOs;

namespace Web.API.Controllers.Subscriptions;

[ApiController]
[Route("subscription-levels")]
public class SubscriptionLevelController : Controller
{
    private readonly ISubscriptionLevelService _subscriptionLevelService;

    public SubscriptionLevelController(ISubscriptionLevelService subscriptionLevelService)
    {
        _subscriptionLevelService = subscriptionLevelService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllSubscriptionLevels()
    {
        var subscriptionLevels = await _subscriptionLevelService.GetAllSubscriptionLevels();
        var result = subscriptionLevels.Select(level => new SubscriptionLevelDto(level))
            .ToList();

        return Json(result);
    }

    [HttpGet]
    [Route("{subscriptionLevelIds}")]
    public async Task<IActionResult> GetSubscriptionLevels(List<int> subscriptionLevelIds)
    {
        var subscriptionLevels = await _subscriptionLevelService.GetSubscriptionLevels(subscriptionLevelIds);
        var result = subscriptionLevels.Select(level => new SubscriptionLevelDto(level));

        return Ok(result);
    }

    [HttpGet]
    [Route("{subscriptionLevelId:int}")]
    public async Task<IActionResult> GetSubscriptionLevel(int subscriptionLevelId)
    {
        var subscriptionLevel = await _subscriptionLevelService.GetSubscriptionLevel(subscriptionLevelId);
        var result = new SubscriptionLevelDto(subscriptionLevel);

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateSubscriptionLevel(string name)
    {
        var subscriptionLevelId = await _subscriptionLevelService.CreateSubscriptionLevel(name);

        return Ok(subscriptionLevelId);
    }

    [HttpPut]
    [Route("{subscriptionLevelId:int}/update/name")]
    public async Task<IActionResult> UpdateName(int subscriptionLevelId, string name)
    {
        await _subscriptionLevelService.UpdateName(subscriptionLevelId, name);

        return Ok();
    }
}