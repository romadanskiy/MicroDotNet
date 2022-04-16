using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RuOverflow.Questions.Infrastructure.Kafka.Data;
using RuOverflow.Questions.Infrastructure.Kafka.Helpers;
using RuOverflow.Questions.Infrastructure.Settings;

namespace RuOverflow.Questions.Infrastructure.Kafka;

public class KafkaInitializer : IHostedService
{
    private readonly KafkaSettings _settings;

    public KafkaInitializer(KafkaSettings settings)
    {
        _settings = settings;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var adminClient =
            new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _settings.Servers }).Build();

        var topics = typeof(KafkaTopics)
            .GetFields()
            .Select(x => x.GetValue(null) as KafkaCreateParameter)
            .Where(x => x is not null)
            .Select(x => x!)
            .GroupBy(x => x.Options, new CreateTopicOptionsComparer()!);

        foreach (var topicGroup in topics)
        {
            try
            {
                await adminClient.CreateTopicsAsync(topicGroup.Select(x => x.Specification), topicGroup.Key);
            }
            catch (Confluent.Kafka.Admin.CreateTopicsException)
            {
                //ignore
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
