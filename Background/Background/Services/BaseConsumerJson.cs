using System.Net;
using Background.Settings;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Background.Services;

public abstract class BaseConsumerJson<TKey, TMessage> : BackgroundService
{
    private readonly KafkaSettings _kafkaSettings;
    protected readonly ILogger<BaseConsumerJson<TKey, TMessage>> Logger;

    public BaseConsumerJson(KafkaSettings kafkaSettings, ILogger<BaseConsumerJson<TKey, TMessage>> logger)
    {
        _kafkaSettings = kafkaSettings;
        Logger = logger;
    }

    protected abstract string Topic { get; }
    protected abstract string GroupId { get; }

    protected virtual ConsumerConfig ConsumerConfig => new()
    {
        BootstrapServers = _kafkaSettings.Servers,
        GroupId = GroupId,
        EnableAutoCommit = false,
        ClientId = Dns.GetHostName(),
    };

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation($"Consumer starts listening {Topic} topic");
        Task.Run(async () =>
        {
            using var consumer = new ConsumerBuilder<TKey, string>(ConsumerConfig).Build();
            consumer.Subscribe(Topic);
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = consumer.Consume(stoppingToken);
                if (message is not null)
                {
                    Logger.LogInformation($"Message recieved from topic: {Topic}");
                    var messageObj = JsonConvert.DeserializeObject<TMessage>(message.Message.Value);
                    await ConsumeAsync(
                        message.Message.Key,
                        messageObj ?? throw new ArgumentException("Could not parse JSON content"),
                        stoppingToken
                    );
                    consumer.Commit(message);
                }
            }

            consumer.Close();
        }, stoppingToken);
        return Task.CompletedTask;
    }

    protected abstract Task ConsumeAsync(TKey key, TMessage message, CancellationToken stoppingToken);
}
