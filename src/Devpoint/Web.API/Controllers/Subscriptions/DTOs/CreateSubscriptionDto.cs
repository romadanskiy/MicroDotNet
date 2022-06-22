using Domain.Developers.Entities;

namespace Web.API.Controllers.Subscriptions.DTOs;

public class CreateSubscriptionDto
{
    public CreateSubscriptionDto(bool isAutoRenewal, int subscriptionLevelId, Guid targetId, EntityType type)
    {
        IsAutoRenewal = isAutoRenewal;
        SubscriptionLevelId = subscriptionLevelId;
        TargetId = targetId;
        Type = type;
    }
    
    public CreateSubscriptionDto() {}
    public bool IsAutoRenewal { get; set; } = false;
    public int SubscriptionLevelId { get; set; }
    public Guid TargetId { get; set; }
    public EntityType Type { get; set; }
}