using System.Text.Json.Nodes;
using Background.Settings;
using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;

namespace Background.Services.QuestionService
{
    public class QuestionConsumer : BaseConsumerJson<Ignore, dynamic>
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        public QuestionConsumer(KafkaSettings kafkaSettings, ILogger<QuestionConsumer> logger,
            ElasticsearchClient elasticsearchClient) :
            base(kafkaSettings, logger)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        protected override string Topic => KafkaTopics.Questions;
        protected override string GroupId => ConsumerGroups.Questions;

        protected override Task ConsumeAsync(Ignore key, dynamic message, CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
