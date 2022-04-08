using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Rating.Handlers;

public class ChangeRatingHandler : IHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity>
{
    public HasRatingEntity Handle(RatingCommands.ChangeRatingCommand input)
    {
        var changeRating = ChangeRatingActionFactory.GetAction(input);
        var entity = input.Entity;
        changeRating(entity);
        return entity;
    }
}

internal static class ChangeRatingActionFactory
{
    public static Action<HasRatingEntity> GetAction(RatingCommands.ChangeRatingCommand command)
    {
        return command switch
        {
            RatingCommands.LikeCommand => entity => entity.Like(),
            RatingCommands.DislikeCommand => entity => entity.Dislike(),
            _ => throw new ArgumentException()
        };
    }
}
