namespace RuOverflow.Questions.Infrastructure.Kafka.Data;

public static class TopicNames
{
    public const string Rating = $"{nameof(RuOverflow)}.rating";

    public const string Delete = $"{nameof(RuOverflow)}.delete";
}
