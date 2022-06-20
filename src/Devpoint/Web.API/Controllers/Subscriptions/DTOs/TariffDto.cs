using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;

namespace Web.API.Controllers.Subscriptions.DTOs;

public class TariffDto
{
    public int Id { get; set; }
    public double PricePerMonth { get; set; }
    public EntityType SubscriptionType { get; set; }
    
    public int SubscriptionLevelId { get; set; }

    public TariffDto(Tariff tariff)
    {
        Id = tariff.Id;
        PricePerMonth = tariff.PricePerMonth;
        SubscriptionType = tariff.SubscriptionType;
        SubscriptionLevelId = tariff.SubscriptionLevelId;
    }
}