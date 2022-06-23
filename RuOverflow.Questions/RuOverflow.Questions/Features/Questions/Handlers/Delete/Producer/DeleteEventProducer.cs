using Confluent.Kafka;
using RuOverflow.Questions.Infrastructure.Kafka;
using RuOverflow.Questions.Infrastructure.Kafka.Base;
using RuOverflow.Questions.Infrastructure.Kafka.Data;

namespace RuOverflow.Questions.Features.Questions.Handlers.Delete.Producer;

public class DeleteEventProducer : BaseProducerJson<DeleteCommand>
{
    public DeleteEventProducer(IProducer<Null, string> producer) : base(producer)
    {
    }

    protected override string Topic => TopicNames.Delete;
}
