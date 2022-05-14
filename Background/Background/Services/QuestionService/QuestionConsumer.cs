using Background.Services.QuestionService.Models;
using Background.Settings;
using Confluent.Kafka;

namespace Background.Services.QuestionService
{
    public class QuestionConsumer : BaseConsumerJson<Ignore, DebeziumPayload>
    {
        public QuestionConsumer(KafkaSettings kafkaSettings, ILogger<QuestionConsumer> logger) :
            base(kafkaSettings, logger)
        {
        }

        protected override string Topic => KafkaTopics.Questions;
        protected override string GroupId => ConsumerGroups.Questions;

        protected override Task ConsumeAsync(Ignore key, DebeziumPayload message, CancellationToken stoppingToken)
        {
            QuestionStore.Questions
                .AddOrUpdate(message.Payload.After.Id, message.Payload.After, (_, _) => message.Payload.After);
            
            return Task.CompletedTask;
        }
    }
}
