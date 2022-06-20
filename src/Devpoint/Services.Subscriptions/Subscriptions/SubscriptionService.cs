using Data.Core;
using Domain.Content.Entities;
using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;
using Domain.Subscriptions.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Services.Developers.Companies;
using Services.Developers.Developers;
using Services.Developers.Projects;
using Services.Subscriptions.Tariffs;

namespace Services.Subscriptions.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly ApplicationContext _context;
    private readonly ITariffService _tariffService;
    private readonly IDeveloperService _developerService;
    private readonly IProjectService _projectService;
    private readonly ICompanyService _companyService;

    public SubscriptionService(ApplicationContext context, ITariffService tariffService, IDeveloperService developerService, IProjectService projectService, ICompanyService companyService)
    {
        _context = context;
        _tariffService = tariffService;
        _developerService = developerService;
        _projectService = projectService;
        _companyService = companyService;
    }

    public IQueryable<Subscription> GetAllSubscriptions()
    {
        var subscriptions = _context.Subscriptions;

        return subscriptions;
    }

    public async Task<Subscription> GetSubscription(int subscriptionId)
    {
        var subscription = await _context.Subscriptions.FindAsync(subscriptionId);

        return subscription;
    }

    public async Task<Subscription> FindSubscription(Guid subscriberId, Guid targetId, EntityType type)
    {
        var subscription = await _context.Subscriptions
            .Include(sub => sub.Tariff)
            .FirstOrDefaultAsync(sub => 
                sub.EntityType == type &&
                sub.SubscriberId == subscriberId &&
                sub.TargetId == targetId
                );

        return subscription;
    }

    public IQueryable<Subscription> GetProjectSubscriptions(Guid id)
    {
        var subscription = _context.Subscriptions
            .Where(sub =>
                sub.EntityType == EntityType.Project &&
                sub.TargetId == id);

        return subscription;
    }

    public IQueryable<Subscription> GetDeveloperSubscriptions(Guid id)
    {
        var subscription = _context.Subscriptions
            .Where(sub =>
                sub.EntityType == EntityType.Developer &&
                sub.TargetId == id);

        return subscription;
    }

    public IQueryable<Subscription> GetCompanySubscriptions(Guid id)
    {
        var subscription = _context.Subscriptions
            .Where(sub =>
                sub.EntityType == EntityType.Company &&
                sub.TargetId == id);

        return subscription;
    }

    public async Task<Tariff> GetSubscriptionTariff(int subscriptionId)
    {
        var subscription = await GetSubscription(subscriptionId);
        _context.Entry(subscription).Reference(s => s.Tariff);

        return subscription.Tariff;
    }

    public async Task<Developer> GetSubscriptionSubscriber(int subscriptionId)
    {
        var subscription = await GetSubscription(subscriptionId);
        _context.Entry(subscription).Reference(s => s.Subscriber);

        return subscription.Subscriber;
    }

    public async Task<int> UserCompanySubscriptionLevel(Guid? userDevId, Guid companyId)
    {
        if (!userDevId.HasValue)
            return 1;

        var sub = await GetCompanySubscriptions(companyId)
            .FirstOrDefaultAsync(sub => sub.Subscriber.Id == userDevId);
        
        if (sub == null)
            return 1;

        await _context.Entry(sub).Reference(s => s.Tariff).LoadAsync();
        return sub.Tariff.SubscriptionLevelId;
    }
    
    public async Task<int> UserDeveloperSubscriptionLevel(Guid? userDevId, Guid developerId)
    {
        if (!userDevId.HasValue)
            return 1;

        var sub = await GetDeveloperSubscriptions(developerId)
            .FirstOrDefaultAsync(sub => sub.Subscriber.Id == userDevId);

        if (sub == null)
            return 1;

        await _context.Entry(sub).Reference(s => s.Tariff).LoadAsync();
        return sub.Tariff.SubscriptionLevelId;
    }
    
    public async Task<int> UserProjectSubscriptionLevel(Guid? userDevId, Guid projectId)
    {
        if (!userDevId.HasValue)
            return 1;

        var sub = await GetProjectSubscriptions(projectId)
            .FirstOrDefaultAsync(sub => sub.Subscriber.Id == userDevId);

        if (sub == null)
            return 1;

        await _context.Entry(sub).Reference(s => s.Tariff).LoadAsync();
        return sub.Tariff.SubscriptionLevelId;
    }

    public bool HasSufficientSubscriptionLevel(Post post, Guid? userDevId, int userSubLevel)
    {
        if (!userDevId.HasValue)
            return post.RequiredSubscriptionLevelId == 1;
        
        return userDevId == post.DeveloperId || userSubLevel >= post.RequiredSubscriptionLevelId;
    }

    public async Task<Subscription> CreateSubscription(DateTime endTime, bool isAutoRenewal, int tariffId, 
        Guid subscriberId, Guid targetId, EntityType type)
    {
        var tariff = await _tariffService.GetTariff(tariffId);
        var subscriber = await _developerService.GetDeveloper(subscriberId);
        var subscription = new Subscription(endTime, isAutoRenewal, tariff, subscriber, targetId, type);
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        return subscription;
    }

    public async Task<int> UserSubscriptionLevel(Guid userId, Guid entityId, EntityType type)
    {
        Subscription sub = await _context.Subscriptions
            .Include(s => s.Tariff)
            .FirstOrDefaultAsync(s => 
                s.EntityType == type &&
                s.Subscriber.Id == userId &&
                s.TargetId == entityId);

        return sub?.Tariff?.SubscriptionLevelId ?? 1;
    }
}