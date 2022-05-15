using Background.Extensions;
using Background.Services.QuestionService;
using Background.Services.RatingService;
using Background.Settings;
using Nest;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var elasticSettings = new ConnectionSettings(new Uri(context.Configuration["ElasticSearch:Host"]));
        services.AddSingleton(new ElasticClient(elasticSettings));
        services.AddSingleton(context.Configuration.GetSettings<KafkaSettings>("Kafka"));
        services.AddSingleton(context.Configuration.GetSettings<RatingConsumerSettings>("Consumers:Rating"));
        services.AddSingleton(context.Configuration.GetSettings<QuestionConsumerSettings>("Consumers:Question"));
        services.AddSingleton(
            context.Configuration.GetSettings<RatingUpdateWorkerSettings>("Workers:RatingUpdateWorker"));
        services.AddSingleton(
            context.Configuration.GetSettings<ElasticUpdateWorkerSettings>("Workers:ElasticUpdateWorker"));
        services.AddHostedService<RatingConsumer>();
        //services.AddHostedService<QuestionConsumer>();
        services.AddHostedService<ElasticUpdateWorker>();
    })
    .Build();

await host.RunAsync();
