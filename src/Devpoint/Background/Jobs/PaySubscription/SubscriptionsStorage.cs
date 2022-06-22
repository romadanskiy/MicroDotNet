namespace Background.Jobs.PaySubscription;

public class SubscriptionsStorage 
{
    private readonly ILogger<SubscriptionsStorage> _logger;

    public SubscriptionsStorage(ILogger<SubscriptionsStorage> logger)
    {
        _logger = logger;
        SubscriptionRecords = new List<SubscriptionRecord>();
    }

    public List<SubscriptionRecord> SubscriptionRecords { get; }

    public void AddSubscription(SubscriptionRecord record)
    {
        SubscriptionRecords.Add(record);
        _logger.LogInformation($"Start subscription: id {record}");
    }

    public void RemoveSubscription(SubscriptionRecord record)
    {
        var recordToRemove = SubscriptionRecords.SingleOrDefault(r => r.SubscriptionId == record.SubscriptionId);

        if (recordToRemove is null)
        {
            _logger.LogError($"There is no subscription with id {record}");
            return;
        }
        
        SubscriptionRecords.Remove(recordToRemove);
        _logger.LogInformation($"Stop subscription: id {record}");
    }
}