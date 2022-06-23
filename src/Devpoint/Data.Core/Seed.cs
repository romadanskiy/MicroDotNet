using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Core;

public static class Seed
{
    private static readonly List<SubscriptionLevel> SubscriptionLevels = new()
    {
        new SubscriptionLevel(1, "Free"),
        new SubscriptionLevel(2, "Basic"),
        new SubscriptionLevel(3, "Improved"),
        new SubscriptionLevel(4, "Pro"),
        new SubscriptionLevel(5, "Team member"),
        new SubscriptionLevel(6, "Owner"),
    };
    
    private static readonly List<Tariff> Tariffs = new()
    {
        new Tariff(1, 0.0d, EntityType.Project, SubscriptionLevels[0].Id),
        new Tariff(2, 9.99d, EntityType.Project, SubscriptionLevels[1].Id),
        new Tariff(3, 19.99d, EntityType.Project, SubscriptionLevels[2].Id),
        new Tariff(4, 39.99d, EntityType.Project, SubscriptionLevels[3].Id),
        new Tariff(5, 0.0d, EntityType.Project, SubscriptionLevels[4].Id),
        new Tariff(6, 0.0d, EntityType.Project, SubscriptionLevels[5].Id),
        
        new Tariff(7, 0.0d, EntityType.Developer, SubscriptionLevels[0].Id),
        new Tariff(8, 9.99d, EntityType.Developer, SubscriptionLevels[1].Id),
        new Tariff(9, 19.99d, EntityType.Developer, SubscriptionLevels[2].Id),
        new Tariff(10, 39.99d, EntityType.Developer, SubscriptionLevels[3].Id),
        new Tariff(11, 0.0d, EntityType.Developer, SubscriptionLevels[4].Id),
        new Tariff(12, 0.0d, EntityType.Developer, SubscriptionLevels[5].Id),
        
        new Tariff(13, 0.0d, EntityType.Company, SubscriptionLevels[0].Id),
        new Tariff(14, 9.99d, EntityType.Company, SubscriptionLevels[1].Id),
        new Tariff(15, 19.99d, EntityType.Company, SubscriptionLevels[2].Id),
        new Tariff(16, 39.99d, EntityType.Company, SubscriptionLevels[3].Id),
        new Tariff(17, 0.0d, EntityType.Company, SubscriptionLevels[4].Id),
        new Tariff(18, 0.0d, EntityType.Company, SubscriptionLevels[5].Id),
    };

    public static void AddSubscriptionLevels(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubscriptionLevel>().HasData(SubscriptionLevels);
    }
    
    public static void AddTariffs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tariff>().HasData(Tariffs);
    }
}