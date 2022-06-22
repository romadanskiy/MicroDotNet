using System.Net;
using Background.Settings;
using Confluent.Kafka;
using NCrontab;
using Newtonsoft.Json;

namespace Background.Services;

public abstract class BaseConsumerJson<TMessage> : BackgroundService
{
    private readonly KafkaSettings _kafkaSettings;
    protected readonly ILogger<BaseConsumerJson<TMessage>> Logger;

    public BaseConsumerJson(KafkaSettings kafkaSettings, ILogger<BaseConsumerJson<TMessage>> logger)
    {
        _kafkaSettings = kafkaSettings;
        Logger = logger;
    }

    protected abstract string Topic { get; }
    protected abstract string GroupId { get; }

    protected abstract int MessagesPerCycle { get; }

    protected abstract string Cron { get; }

    protected abstract TimeSpan MaxTimeWithoutProcessing { get; }

    protected virtual ConsumerConfig ConsumerConfig => new()
    {
        BootstrapServers = _kafkaSettings.Servers,
        GroupId = GroupId,
        EnableAutoCommit = false,
        AutoOffsetReset = AutoOffsetReset.Earliest,
        ClientId = Dns.GetHostName(),
    };

    protected virtual Task BeforeStart()
    {
        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation($"Consumer starts listening {Topic} topic");
        Task.Run(async () =>
        {
            try
            {
                await BeforeStart();
                using var adminClient = new AdminClientBuilder(new AdminClientConfig()
                {
                    BootstrapServers = _kafkaSettings.Servers,
                }).Build();
                await WaitUntilTopicCreatedAsync(stoppingToken, adminClient);
                using var consumer = new ConsumerBuilder<Ignore, string>(ConsumerConfig).Build();
                consumer.Subscribe(Topic);
                var messages = new List<TMessage>();
                var lastConsumedDate = DateTime.UtcNow;
                while (!stoppingToken.IsCancellationRequested)
                {
                    var message = consumer.Consume(MaxTimeWithoutProcessing - (DateTime.UtcNow - lastConsumedDate));
                    if (message is not null)
                    {
                        Logger.LogInformation($"Message recieved from topic: {Topic}");
                        if (message.Message.Value is null)
                        {
                            consumer.Commit(message);
                        }
                        else
                        {
                            var messageObj = JsonConvert.DeserializeObject<TMessage>(message.Message.Value);
                            messages.Add(messageObj ?? throw new ArgumentException("Could not parse JSON content"));
                            lastConsumedDate = messages.Any() ? lastConsumedDate : DateTime.UtcNow;
                        }
                    }

                    if (messages.Any() && (messages.Count >= MessagesPerCycle ||
                                           DateTime.UtcNow - lastConsumedDate >= MaxTimeWithoutProcessing))
                    {
                        try
                        {
                            await ConsumeAsync(messages, stoppingToken);
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(
                                $"Message with offset = {message?.Offset} fails. Exception message: {e.Message}");
                        }

                        if (message is null)
                        {
                            consumer.Commit();
                        }
                        else
                        {
                            consumer.Commit(message);
                        }

                        messages.Clear();
                        await Task.Delay(GetTimeBeforeNextRun(), stoppingToken);
                    }
                }

                consumer.Close();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
        }, stoppingToken);
        return Task.CompletedTask;
    }

    private async Task WaitUntilTopicCreatedAsync(CancellationToken stoppingToken, IAdminClient adminClient)
    {
        while (!adminClient.GetMetadata(TimeSpan.FromMinutes(5)).Topics.Select(x => x.Topic).Contains(Topic))
        {
            Logger.LogWarning($"Can't find {Topic} topic");
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private TimeSpan GetTimeBeforeNextRun() =>
        CrontabSchedule.Parse(Cron).GetNextOccurrence(DateTime.UtcNow) - DateTime.UtcNow;

    protected abstract Task ConsumeAsync(List<TMessage> messages, CancellationToken stoppingToken);
}
