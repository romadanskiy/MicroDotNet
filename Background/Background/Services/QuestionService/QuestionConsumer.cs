using Background.Services.QuestionService.Models;
using Background.Settings;
using Nest;

namespace Background.Services.QuestionService
{
    public class QuestionConsumer : BaseConsumerJson<DebeziumPayload>
    {
        private readonly QuestionConsumerSettings _settings;
        private readonly ElasticClient _elasticClient;

        public QuestionConsumer(KafkaSettings kafkaSettings, ILogger<QuestionConsumer> logger,
            QuestionConsumerSettings settings, ElasticClient elasticClient) :
            base(kafkaSettings, logger)
        {
            _settings = settings;
            _elasticClient = elasticClient;
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
