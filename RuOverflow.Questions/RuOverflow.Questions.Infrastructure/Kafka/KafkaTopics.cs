using Confluent.Kafka.Admin;
using RuOverflow.Questions.Infrastructure.Kafka.Data;

namespace RuOverflow.Questions.Infrastructure.Kafka;

public static class KafkaTopics
{
    public static readonly KafkaCreateParameter Rating = new KafkaCreateParameter(new TopicSpecification()
    {
        Name = TopicNames.Rating,
        NumPartitions = 1,
        ReplicationFactor = 1,
        Configs = new Dictionary<string, string>()
        {
            { "delete.retention.ms", "7200000" },
        }
    });
}
