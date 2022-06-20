using Data.Core;
using Domain.Subscriptions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services.Subscriptions.SubscriptionLevels;

public class SubscriptionLevelService : ISubscriptionLevelService
{
    private readonly ApplicationContext _context;

    public SubscriptionLevelService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<SubscriptionLevel>> GetAllSubscriptionLevels()
    {
        var subscriptionLevels = await _context.SubscriptionLevels.ToListAsync();

        return subscriptionLevels;
    }

    public async Task<List<SubscriptionLevel>> GetSubscriptionLevels(List<int> subscriptionLevelIds)
    {
        var subscriptionLevels = await _context
            .SubscriptionLevels
            .Where(level => subscriptionLevelIds.Contains(level.Id))
            .ToListAsync();

        return subscriptionLevels;
    }

    public async Task<SubscriptionLevel> GetSubscriptionLevel(int subscriptionLevelId)
    {
        var subscriptionLevel = await _context.SubscriptionLevels.FindAsync(subscriptionLevelId);

        return subscriptionLevel;
    }

    public async Task<int> CreateSubscriptionLevel(string name)
    {
        var subscriptionLevel = new SubscriptionLevel(name);
        _context.SubscriptionLevels.Add(subscriptionLevel);
        await _context.SaveChangesAsync();

        return subscriptionLevel.Id;
    }

    public async Task UpdateName(int subscriptionLevelId, string name)
    {
        var subscriptionLevel = await GetSubscriptionLevel(subscriptionLevelId);
        subscriptionLevel.Name = name;
        await _context.SaveChangesAsync();
    }
}