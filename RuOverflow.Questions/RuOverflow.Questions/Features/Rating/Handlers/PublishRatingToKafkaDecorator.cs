using RuOverflow.Questions.Features.Rating.Producer;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Rating.Handlers;

[Decorator]
public class PublishRatingToKafkaDecorator : IAsyncHandler<ChangeRatingCommand>
{
    private readonly IAsyncHandler<ChangeRatingCommand> _handler;
    private readonly RatingProducer _ratingProducer;

    public PublishRatingToKafkaDecorator(IAsyncHandler<ChangeRatingCommand> handler, RatingProducer ratingProducer)
    {
        _handler = handler;
        _ratingProducer = ratingProducer;
    }

    public async Task Handle(ChangeRatingCommand input)
    {
        _ratingProducer.Publish(input);
        await _handler.Handle(input);
    }
}
