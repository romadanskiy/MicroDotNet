using Background.Rabbit;

namespace Background.Jobs.PaySubscription;

public class PaySubscriptionJob : CronJob
{
    private readonly ILogger<PaySubscriptionJob> _logger;
    private readonly List<PaySubscriptionRecord> _records;
    private readonly IRabbitPublisher _publisher;

    public PaySubscriptionJob(ILogger<PaySubscriptionJob> logger, IRabbitPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
        _records = new List<PaySubscriptionRecord>();
        Start();
    }

    public void AddSubscription(PaySubscriptionDto dto)
    {
        var record = new PaySubscriptionRecord(dto);
        _records.Add(record);
        
        _logger.LogInformation($"Start subscription: {record}");
    }

    protected override void DoWork()
    {
        foreach (var record in _records)
        {
            _publisher.SendMessage(record);
            _logger.LogInformation($"Withdrawal: {record}");
        }
    }

    public void RemoveSubscription(PaySubscriptionDto dto)
    {
        var record = _records.SingleOrDefault(record => record.IsEqualTo(dto));

        if (record is null)
        {
            record = new PaySubscriptionRecord(dto);
            _logger.LogError($"There is no subscription with {record}");
            return;
        }
        
        _records.Remove(record);
        _logger.LogInformation($"Stop subscription: {record}");
    }
}