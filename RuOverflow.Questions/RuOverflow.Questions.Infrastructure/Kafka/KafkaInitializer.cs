using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RuOverflow.Questions.Infrastructure.Kafka.Data;
using RuOverflow.Questions.Infrastructure.Kafka.Helpers;

namespace RuOverflow.Questions.Infrastructure.Kafka;

public class KafkaInitializer : IHostedService
{
    private readonly string _servers;

    public KafkaInitializer(IConfiguration configuration)
    {
        _servers = configuration.GetSection("kafka:servers").Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var adminClient =
            new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _servers }).Build();

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
            catch(Confluent.Kafka.Admin.CreateTopicsException)
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
