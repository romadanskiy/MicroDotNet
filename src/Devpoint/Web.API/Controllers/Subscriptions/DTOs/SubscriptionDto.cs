using Domain.Developers.Entities;
using Domain.Subscriptions.Entities.Subscriptions;
using Web.API.Controllers.Developers.DTOs;

namespace Web.API.Controllers.Subscriptions.DTOs;

public class SubscriptionDto
{
    public int Id { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAutoRenewal { get; set; }
    
    public TariffDto Tariff { get; set; }
    
    public EntityDto Entity { get; set; }

    public EntityType EntityType { get; set; }

    public SubscriptionDto(Subscription subscription)
    {
        Id = subscription.Id;
        EndTime = subscription.EndTime;
        IsAutoRenewal = subscription.IsAutoRenewal;
        Tariff = new TariffDto(subscription.Tariff);
        EntityType = subscription.EntityType;
    }
}