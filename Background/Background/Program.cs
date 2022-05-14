using Background.Extensions;
using Background.Services.QuestionService;
using Background.Services.RatingService;
using Background.Settings;
using Elastic.Clients.Elasticsearch;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var elasticSettings = new ElasticsearchClientSettings(new Uri(context.Configuration["ElasticSearch:Host"]));
        services.AddSingleton(new ElasticsearchClient(elasticSettings));
        services.AddSingleton(context.Configuration.GetSettings<KafkaSettings>("Kafka"));
        services.AddSingleton(
            context.Configuration.GetSettings<RatingUpdateWorkerSettings>("Workers:RatingUpdateWorker"));
        services.AddSingleton(
            context.Configuration.GetSettings<ElasticUpdateWorkerSettings>("Worker:ElasticUpdateWorker"));
        services.AddHostedService<RatingConsumer>();
        services.AddHostedService<QuestionConsumer>();
        services.AddHostedService<RatingUpdateWorker>();
    })
    .Build();

await host.RunAsync();
