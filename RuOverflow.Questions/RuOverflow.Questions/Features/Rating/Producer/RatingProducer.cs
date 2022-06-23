using Confluent.Kafka;
using RuOverflow.Questions.Infrastructure.Kafka;
using RuOverflow.Questions.Infrastructure.Kafka.Base;
using RuOverflow.Questions.Infrastructure.Kafka.Data;

namespace RuOverflow.Questions.Features.Rating.Producer;

public class RatingProducerJson : BaseProducerJson<ChangeRatingCommand>
{
    public RatingProducerJson(IProducer<Null, string> producer) : base(producer)
    {
    }

    protected override string Topic => TopicNames.Rating;
}
