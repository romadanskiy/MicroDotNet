using Background.Settings;
using Nest;

namespace Background.Services.QuestionService
{
    public class ElasticUpdateWorker : BaseWorker
    {
        private readonly ElasticClient _elasticClient;

        public ElasticUpdateWorker(ILogger<ElasticUpdateWorker> logger, ElasticUpdateWorkerSettings settings,
            ElasticClient elasticClient) :
            base(logger)
        {
            _elasticClient = elasticClient;
            Cron = settings.Cron;
        }

        protected override string Cron { get; }

        protected override async Task RunAsync(CancellationToken stoppingToken)
        {
            foreach (var key in QuestionStore.Questions.Keys)
            {
                if (QuestionStore.Questions.TryRemove(key, out var question))
                {
                    await _elasticClient.CreateAsync(question, descriptor => descriptor.Index(question.UserId),
                        stoppingToken);
                }
            }
        }
    }
}
