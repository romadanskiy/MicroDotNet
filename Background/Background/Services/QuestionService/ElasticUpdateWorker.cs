using Background.Settings;
using Elastic.Clients.Elasticsearch;

namespace Background.Services.QuestionService
{
    public class ElasticUpdateWorker : BaseWorker
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        public ElasticUpdateWorker(ILogger<ElasticUpdateWorker> logger, ElasticUpdateWorkerSettings settings,
            ElasticsearchClient elasticsearchClient) :
            base(logger)
        {
            _elasticsearchClient = elasticsearchClient;
            Cron = settings.Cron;
        }

        protected override string Cron { get; }

        protected override Task RunAsync(CancellationToken stoppingToken)
        {
            QuestionStore.Questions.Clear();
            return Task.CompletedTask;
        }
    }
}
