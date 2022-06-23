using Confluent.Kafka.Admin;

namespace RuOverflow.Questions.Infrastructure.Kafka.Data;

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

    public static readonly KafkaCreateParameter Delete = new KafkaCreateParameter(new TopicSpecification()
    {
        Name = TopicNames.Delete,
        NumPartitions = 2,
        ReplicationFactor = 2,
        Configs = new Dictionary<string, string>()
        {
            { "delete.retention.ms", "7200000" },
        }
    });
}
