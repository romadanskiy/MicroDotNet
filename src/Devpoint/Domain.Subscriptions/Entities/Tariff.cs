using Domain.Developers.Entities;

namespace Domain.Subscriptions.Entities;

public class Tariff
{
    public int Id { get; set; }
    public double PricePerMonth { get; set; }
    public EntityType SubscriptionType { get; set; }
    public int SubscriptionLevelId { get; set; }
    public SubscriptionLevel SubscriptionLevel { get; set; }

    public Tariff(int id, double pricePerMonth, EntityType subscriptionType, int subscriptionLevelId)
    {
        Id = id;
        PricePerMonth = pricePerMonth;
        SubscriptionType = subscriptionType;
        SubscriptionLevelId = subscriptionLevelId;
    }
    
    public Tariff(double pricePerMonth, EntityType subscriptionType, SubscriptionLevel subscriptionLevel)
    {
        PricePerMonth = pricePerMonth;
        SubscriptionType = subscriptionType;
        SubscriptionLevel = subscriptionLevel;
    }
    
    private Tariff() {}
}