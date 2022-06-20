using Domain.Content.Entities;
using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;
using Domain.Subscriptions.Entities.Subscriptions;

namespace Services.Subscriptions.Subscriptions;

public interface ISubscriptionService
{
    public IQueryable<Subscription> GetAllSubscriptions();
    
    public Task<Subscription> GetSubscription(int subscriptionId);
    
    public Task<Subscription> FindSubscription(Guid subscriberId, Guid targetId, EntityType type);

    public IQueryable<Subscription> GetProjectSubscriptions(Guid projectId);

    public IQueryable<Subscription> GetDeveloperSubscriptions(Guid developerId);

    public IQueryable<Subscription> GetCompanySubscriptions(Guid companyId);

    public Task<Tariff> GetSubscriptionTariff(int subscriptionId);

    public Task<Developer> GetSubscriptionSubscriber(int subscriptionId);

    public Task<int> UserCompanySubscriptionLevel(Guid? userDevId, Guid companyId);
    public Task<int> UserDeveloperSubscriptionLevel(Guid? userDevId, Guid developerId);
    public Task<int> UserProjectSubscriptionLevel(Guid? userDevId, Guid projectId);
    public bool HasSufficientSubscriptionLevel(Post post, Guid? userDevId, int userSubLevel);
    
    public Task<int> UserSubscriptionLevel(Guid userId, Guid entityId, EntityType type);

    public Task<Subscription> CreateSubscription(DateTime endTime, bool isAutoRenewal, int tariffId, Guid subscriberId,
        Guid targetId, EntityType type);
}