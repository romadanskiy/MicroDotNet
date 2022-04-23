using RuOverflow.Questions.Features.Questions.Handlers.Delete.Producer;
using RuOverflow.Questions.Infrastructure.Handlers;
using RuOverflow.Questions.Infrastructure.Kafka.Base;

namespace RuOverflow.Questions.Features.Questions.Handlers.Delete;

[Decorator]
public class DeleteEventPublishDecorator : IAsyncHandler<DeleteCommand>
{
    private readonly IAsyncHandler<DeleteCommand> _handler;
    private readonly IProducer<DeleteCommand> _producer;

    public DeleteEventPublishDecorator(IAsyncHandler<DeleteCommand> handler, IProducer<DeleteCommand> producer)
    {
        _handler = handler;
        _producer = producer;
    }

    public async Task Handle(DeleteCommand input)
    {
        await _handler.Handle(input);
        await _producer.Publish(input);
    }
}
