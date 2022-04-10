using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Rating.Handlers;

[Decorator]
public class PublishRatingToKafkaDecorator : IAsyncHandler<ChangeRatingCommand>
{
    private readonly IAsyncHandler<ChangeRatingCommand> _handler;

    public PublishRatingToKafkaDecorator(IAsyncHandler<ChangeRatingCommand> handler)
    {
        _handler = handler;
    }

    public async Task Handle(ChangeRatingCommand input)
    {
        //todo publish to kafka here
        await _handler.Handle(input);
    }
}
