using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Rating.Handlers;

public class ChangeRatingHandler : IHandler<RatingCommands.ChangeRatingCommand, IHasRating>
{
    public IHasRating Handle(RatingCommands.ChangeRatingCommand input)
    {
        var changeRating = ChangeRatingActionFactory.GetAction(input);
        var entity = input.Entity;
        changeRating(entity);
        return entity;
    }
}

static class ChangeRatingActionFactory
{
    public static Action<IHasRating> GetAction(RatingCommands.ChangeRatingCommand command) =>
        command switch
        {
            RatingCommands.LikeCommand => entity => entity.Like(),
            RatingCommands.DislikeCommand => entity => entity.Dislike(),
            _ => throw new ArgumentException()
        };
}
