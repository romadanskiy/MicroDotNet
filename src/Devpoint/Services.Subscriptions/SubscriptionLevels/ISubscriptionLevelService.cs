using Domain.Subscriptions.Entities;

namespace Services.Subscriptions.SubscriptionLevels;

public interface ISubscriptionLevelService
{
    public Task<List<SubscriptionLevel>> GetAllSubscriptionLevels();

    public Task<List<SubscriptionLevel>> GetSubscriptionLevels(List<int> subscriptionLevelIds);

    public Task<SubscriptionLevel> GetSubscriptionLevel(int subscriptionLevelId);

    public Task<int> CreateSubscriptionLevel(string name);

    public Task UpdateName(int subscriptionLevelId, string name);
}