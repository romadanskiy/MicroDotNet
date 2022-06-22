using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Subscriptions.Tariffs;
using Web.API.Controllers.Subscriptions.DTOs;

namespace Web.API.Controllers.Subscriptions;

[ApiController]
[Route("tariffs")]
public class TariffController : Controller
{
    private readonly ITariffService _tariffService;

    public TariffController(ITariffService tariffService)
    {
        _tariffService = tariffService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllTariffs()
    {
        var tariffs = await _tariffService.GetAllTariffs();
        var result = tariffs.Select(t => new TariffDto(t));

        return Ok(result);
    }

    [HttpGet]
    [Route("{tariffIds}")]
    public async Task<IActionResult> GetTariffs(List<int> tariffIds)
    {
        var tariffs = await _tariffService.GetTariffs(tariffIds);
        var result = tariffs.Select(t => new TariffDto(t));

        return Ok(result);
    }

    [HttpGet]
    [Route("{tariffId:int}")]
    public async Task<IActionResult> GetTariff(int tariffId)
    {
        var tariff = await _tariffService.GetTariff(tariffId);
        var result = new TariffDto(tariff);

        return Ok(result);
    }

    [HttpGet]
    [Route("{tariffId:int}/SubscriptionLevel")]
    public async Task<IActionResult> GetTariffSubscriptionLevel(int tariffId)
    {
        var subscriptionLevel = await _tariffService.GetTariffSubscriptionLevel(tariffId);
        var result = new SubscriptionLevelDto(subscriptionLevel);

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateTariff(int pricePerMonth, EntityType subscriptionType,
        int subscriptionLevelId)
    {
        var tariffId = await _tariffService.CreateTariff(pricePerMonth, subscriptionType, subscriptionLevelId);

        return Ok(tariffId);
    }

    [HttpPut]
    [Route("{tariffId:int}/update/price-per-month")]
    public async Task<IActionResult> UpdatePricePerMonth(int tariffId, int pricePerMonth)
    {
        await _tariffService.UpdatePricePerMonth(tariffId, pricePerMonth);

        return Ok();
    }
}