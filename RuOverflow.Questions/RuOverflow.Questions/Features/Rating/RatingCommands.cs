namespace RuOverflow.Questions.Features.Rating;

public class RatingCommands
{
    public abstract record ChangeRatingCommand(IHasRating Entity);

    public record LikeCommand(IHasRating Entity) : ChangeRatingCommand(Entity);

    public record DislikeCommand(IHasRating Entity) : ChangeRatingCommand(Entity);
}
