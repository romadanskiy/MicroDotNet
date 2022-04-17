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
        AutoOffsetReset = AutoOffsetReset.Earliest,
    };

    protected IConsumer<TKey, string> Consumer =>
        new ConsumerBuilder<TKey, string>(ConsumerConfig)
            .Build();

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Consumer.Subscribe(Topic);
        Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = Consumer.Consume();
                if (message is not null)
                {
                    var messageObj = JsonConvert.DeserializeObject<TMessage>(message.Message.Value);
                    await ConsumeAsync(
                        message.Message.Key,
                        messageObj ?? throw new ArgumentException("Could not parse JSON content"),
                        stoppingToken
                    );
                    Consumer.Commit(message);
                }
            }
        }, stoppingToken);
        return Task.CompletedTask;
    }

    protected abstract Task ConsumeAsync(TKey key, TMessage message, CancellationToken stoppingToken);

    public override void Dispose()
    {
        Consumer.Dispose();
        base.Dispose();
    }
}
