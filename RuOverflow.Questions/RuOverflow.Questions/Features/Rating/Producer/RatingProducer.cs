using System.Net;
using Confluent.Kafka;
using Newtonsoft.Json;
using RuOverflow.Questions.Infrastructure.Kafka;
using RuOverflow.Questions.Infrastructure.Kafka.Data;
using RuOverflow.Questions.Settings;

namespace RuOverflow.Questions.Features.Rating.Producer;

public class RatingProducer : KafkaBaseProducer
{
    private readonly KafkaSettings _kafkaSettings;

    public RatingProducer(KafkaSettings kafkaSettings)
    {
        _kafkaSettings = kafkaSettings;
    }

    public void Publish(ChangeRatingCommand message)
    {
        using var producer = new ProducerBuilder<Null, string>(new ProducerConfig()
        {
            BootstrapServers = _kafkaSettings.Servers,
            ClientId = Dns.GetHostName(),
            Acks = Acks.None,
        }).Build();

        producer.Produce(TopicNames.Rating, new Message<Null, string>()
        {
            Value = JsonConvert.SerializeObject(message),
        });
    }
}
