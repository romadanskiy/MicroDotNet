namespace Background.Services.RatingWorker;

public record ChangeRatingMessage(Guid EntityId, EntityWithRatingType EntityType, int Amount);

public enum EntityWithRatingType
{
    Question,
    Answer
}
