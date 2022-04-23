using RuOverflow.Questions.Features.Rating.Producer;
using RuOverflow.Questions.Infrastructure.Handlers;
using RuOverflow.Questions.Infrastructure.Kafka;
using RuOverflow.Questions.Infrastructure.Kafka.Base;

namespace RuOverflow.Questions.Features.Rating.Handlers;

[Decorator]
public class PublishRatingDecorator : IAsyncHandler<ChangeRatingCommand>
{
    private readonly IAsyncHandler<ChangeRatingCommand> _handler;
    private readonly IProducer<ChangeRatingCommand> _ratingProducerJson;

    public PublishRatingDecorator(IAsyncHandler<ChangeRatingCommand> handler, IProducer<ChangeRatingCommand> ratingProducerJson)
    {
        _handler = handler;
        _ratingProducerJson = ratingProducerJson;
    }

    public async Task Handle(ChangeRatingCommand input)
    {
        await _ratingProducerJson.Publish(input);
        await _handler.Handle(input);
    }
}
