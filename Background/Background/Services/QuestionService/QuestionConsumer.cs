using Background.Services.QuestionService.Models;
using Background.Settings;

namespace Background.Services.QuestionService
{
    public class QuestionConsumer : BaseConsumerJson<DebeziumPayload>
    {
        private readonly QuestionConsumerSettings _settings;

        public QuestionConsumer(KafkaSettings kafkaSettings, ILogger<QuestionConsumer> logger,
            QuestionConsumerSettings settings) :
            base(kafkaSettings, logger)
        {
            _settings = settings;
        }

        protected override string Topic => KafkaTopics.Questions;
        protected override string GroupId => ConsumerGroups.Questions;
        protected override int MessagesPerCycle => _settings.MessagesPerCycle;
        protected override string Cron => _settings.Cron;

        protected override TimeSpan MaxTimeWithoutProcessing =>
            TimeSpan.FromMinutes(_settings.MaxTimeWithoutProcessing);

        protected override Task ConsumeAsync(List<DebeziumPayload> messages, CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
