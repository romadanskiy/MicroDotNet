using System.Collections.Concurrent;

namespace Background.Services.RatingService;

public static class RatingStore
{
    public static readonly ConcurrentDictionary<Guid, int> Questions = new();
    public static readonly ConcurrentDictionary<Guid, int> Answers = new();

    public static void Update(ChangeRatingMessage message)
    {
        var store = GetStore(message.EntityType);
        store.AddOrUpdate(message.EntityId, _ => message.Amount, (_, i) => i += message.Amount);
    }

    private static ConcurrentDictionary<Guid, int> GetStore(EntityWithRatingType entityType) =>
        entityType switch
        {
            EntityWithRatingType.Answer => Answers,
            EntityWithRatingType.Question => Questions,
            _ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null)
        };
}
