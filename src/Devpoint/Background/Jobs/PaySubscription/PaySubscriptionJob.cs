using Background.Rabbit;

namespace Background.Jobs.PaySubscription;

public class PaySubscriptionJob : CronJob
{
    private readonly ILogger<PaySubscriptionJob> _logger;
    private readonly List<SubscriptionRecord> _records;
    private readonly IRabbitPublisher _publisher;

    public PaySubscriptionJob(ILogger<PaySubscriptionJob> logger, IRabbitPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
        _records = new List<SubscriptionRecord>();
        Start();
    }

    public void AddSubscription(SubscriptionRecord record)
    {
        _records.Add(record);
        _logger.LogInformation($"Start subscription: id {record}");
    }

    protected override void DoWork()
    {
        foreach (var record in _records)
        {
            _publisher.SendMessage(record);
            _logger.LogInformation($"Withdrawal: id {record}");
        }
    }

    public void RemoveSubscription(SubscriptionRecord record)
    {
        var recordToRemove = _records.SingleOrDefault(r => r.SubscriptionId == record.SubscriptionId);

        if (recordToRemove is null)
        {
            _logger.LogError($"There is no subscription with id {record}");
            return;
        }
        
        _records.Remove(record);
        _logger.LogInformation($"Stop subscription: id {record}");
    }
}