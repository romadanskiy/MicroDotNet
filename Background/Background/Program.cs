using Background.Extensions;
using Background.Services.RatingWorker;
using Background.Settings;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton(context.Configuration.GetSettings<KafkaSettings>("Kafka"));
        services.AddSingleton(
            context.Configuration.GetSettings<RatingUpdateWorkerSettings>("Workers:RatingUpdateWorker"));
        services.AddHostedService<RatingConsumer>();
        services.AddHostedService<RatingUpdateWorker>();
    })
    .Build();

await host.RunAsync();
