using Background.Rabbit;
using System;

namespace Background.Jobs.PaySubscription;

public class PaySubscriptionJob : CronJob
{
    private readonly ILogger<PaySubscriptionJob> _logger;
    private readonly SubscriptionsStorage _storage;
    private readonly IRabbitPublisher _publisher;

    public PaySubscriptionJob(ILogger<PaySubscriptionJob> logger, IRabbitPublisher publisher, SubscriptionsStorage storage)
    {
        _logger = logger;
        _publisher = publisher;
        _storage = storage;
    }

    public override Task DoWork(CancellationToken cancellationToken)
    {
        foreach (var record in _storage.SubscriptionRecords.ToList())
        {
            _publisher.SendMessage(record);
            _logger.LogInformation($"Withdrawal: id {record}");
        }
        
        return Task.CompletedTask;
    }
}