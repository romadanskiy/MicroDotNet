using Confluent.Kafka.Admin;

namespace RuOverflow.Questions.Infrastructure.Kafka.Data;

public class KafkaCreateParameter
{
    public KafkaCreateParameter(TopicSpecification specification)
    {
        Specification = specification;
    }

    public TopicSpecification Specification { get; set; }
    public CreateTopicsOptions? Options { get; set; }
}
