using Domain.Developers.Entities;

namespace Domain.Subscriptions.Entities.Subscriptions;

public class Subscription
{
    public int Id { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAutoRenewal { get; set; }
    public Tariff Tariff { get; set; }
    public Developer Subscriber { get; set; }
    public Guid SubscriberId { get; set; }
    
    public Guid TargetId { get; set; }
    public EntityType EntityType { get; set; }

    public Subscription(
        DateTime endTime, bool isAutoRenewal, Tariff tariff, 
        Developer subscriber, Guid targetId, EntityType entityType)
    {
        EndTime = endTime;
        IsAutoRenewal = isAutoRenewal;
        Tariff = tariff;
        Subscriber = subscriber;
        TargetId = targetId;
        EntityType = entityType;
    }
    
    public Subscription()
    {
    }
}