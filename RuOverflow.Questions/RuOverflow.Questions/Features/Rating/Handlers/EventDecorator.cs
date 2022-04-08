using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Rating.Handlers;

[Decorator]
public class EventDecorator : IAsyncHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity>
{
    private readonly IAsyncHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity> _handler;

    public EventDecorator(IAsyncHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity> handler)
    {
        _handler = handler;
    }

    public async Task<HasRatingEntity> Handle(RatingCommands.ChangeRatingCommand input)
    {
        var entity = await _handler.Handle(input);
        //todo publish to kafka
        return entity;
    }
}
