namespace RuOverflow.Questions.Features.Rating;

public record ChangeRatingMutation(Guid EntityWithRatingId);

public record LikeMutation(Guid EntityWithRatingId) : ChangeRatingMutation(EntityWithRatingId);

public record DislikeMutation(Guid EntityWithRatingId) : ChangeRatingMutation(EntityWithRatingId);
