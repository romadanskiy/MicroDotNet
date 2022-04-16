using System.Net;
using Confluent.Kafka;
using Newtonsoft.Json;
using RuOverflow.Questions.Infrastructure.Kafka;
using RuOverflow.Questions.Infrastructure.Kafka.Data;
using RuOverflow.Questions.Infrastructure.Settings;

namespace RuOverflow.Questions.Features.Rating.Producer;

public class RatingProducer : KafkaBaseProducer
{
    private readonly IProducer<Null, string> _producer;

    public RatingProducer(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    public void Publish(ChangeRatingCommand message)
    {
        _producer.Produce(TopicNames.Rating, new Message<Null, string>()
        {
            Value = JsonConvert.SerializeObject(message),
        });
    }
}
