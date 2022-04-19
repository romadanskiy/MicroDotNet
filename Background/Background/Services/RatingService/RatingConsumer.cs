using Background.Settings;
using Confluent.Kafka;

namespace Background.Services.RatingService;

public class RatingConsumer : BaseConsumerJson<Ignore, ChangeRatingMessage>
{
    public RatingConsumer(KafkaSettings kafkaSettings, ILogger<RatingConsumer> logger) : base(kafkaSettings, logger)
    {
    }

    protected override string Topic => KafkaTopics.Rating;
    protected override string GroupId => ConsumerGroups.Rating;

    protected override Task ConsumeAsync(Ignore key, ChangeRatingMessage message, CancellationToken stoppingToken)
    {
        RatingStore.Update(message);
        return Task.CompletedTask;
    }
}
