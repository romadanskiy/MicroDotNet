using Domain.Subscriptions.Entities;

namespace Web.API.Controllers.Subscriptions.DTOs;

public class SubscriptionLevelDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public SubscriptionLevelDto(SubscriptionLevel subscriptionLevel)
    {
        Id = subscriptionLevel.Id;
        Name = subscriptionLevel.Name;
    }
}