namespace RuOverflow.Questions.Features.Rating;

public record ChangeRatingCommand(Guid EntityId, EntityWithRatingType EntityType, int Amount);

public enum EntityWithRatingType
{
    Question,
    Answer
}
