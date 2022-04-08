namespace RuOverflow.Questions.Features.Rating;

public class RatingCommands
{
    public abstract record ChangeRatingCommand(HasRatingEntity Entity);

    public record LikeCommand(HasRatingEntity Entity) : ChangeRatingCommand(Entity);

    public record DislikeCommand(HasRatingEntity Entity) : ChangeRatingCommand(Entity);
}
