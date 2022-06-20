using Data.Core;
using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Subscriptions.SubscriptionLevels;

namespace Services.Subscriptions.Tariffs;

public class TariffService : ITariffService
{
    private readonly ApplicationContext _context;
    private readonly ISubscriptionLevelService _subscriptionLevelService;

    public TariffService(ApplicationContext context, ISubscriptionLevelService subscriptionLevelService)
    {
        _context = context;
        _subscriptionLevelService = subscriptionLevelService;
    }

    public async Task<List<Tariff>> GetAllTariffs()
    {
        var tariffs = await _context.Tariffs.ToListAsync();

        return tariffs;
    }

    public async Task<List<Tariff>> GetTariffs(List<int> tariffIds)
    {
        var tariffs = await _context.Tariffs.Where(tariff => tariffIds.Contains(tariff.Id)).ToListAsync();

        return tariffs;
    }

    public async Task<Tariff> GetTariff(int tariffId)
    {
        var tariff = await _context.Tariffs.FindAsync(tariffId);

        return tariff;
    }

    public async Task<Tariff> GetTariff(int subLevelId, EntityType subscriptionType)
    {
        var tariff = await _context.Tariffs.FirstOrDefaultAsync(t =>
            t.SubscriptionLevelId == subLevelId && t.SubscriptionType == subscriptionType);

        return tariff;
    }

    public async Task<SubscriptionLevel> GetTariffSubscriptionLevel(int tariffId)
    {
        var tariff = await GetTariff(tariffId);
        await _context.Entry(tariff).Reference(t => t.SubscriptionLevel).LoadAsync();

        return tariff.SubscriptionLevel;
    }

    public async Task<int> CreateTariff(int pricePerMonth, EntityType subscriptionType, int subscriptionLevelId)
    {
        var subscriptionLevel = await _subscriptionLevelService.GetSubscriptionLevel(subscriptionLevelId);
        var tariff = new Tariff(pricePerMonth, subscriptionType, subscriptionLevel);
        _context.Tariffs.Add(tariff);
        await _context.SaveChangesAsync();

        return tariff.Id;
    }

    public async Task UpdatePricePerMonth(int tariffId, int pricePerMonth)
    {
        var tariff = await GetTariff(tariffId);
        tariff.PricePerMonth = pricePerMonth;
        await _context.SaveChangesAsync();
    }
}