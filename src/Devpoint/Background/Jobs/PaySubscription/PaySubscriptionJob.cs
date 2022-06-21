namespace Background.Jobs.PaySubscription;

public class PaySubscriptionJob : CronJob
{
    private readonly ILogger<PaySubscriptionJob> _logger;
    private readonly List<PaySubscriptionRecord> _records;

    public PaySubscriptionJob(ILogger<PaySubscriptionJob> logger)
    {
        _logger = logger;
        _records = new List<PaySubscriptionRecord>();
        Start();
    }

    public void AddSubscription(PaySubscriptionDto dto)
    {
        var record = new PaySubscriptionRecord(dto);
        _records.Add(record);
        
        _logger.LogInformation($"Start subscription: {record}");
    }

    protected override Task DoWork()
    {
        foreach (var record in _records)
        {
            //send message
            _logger.LogInformation($"Withdrawal: {record}");
        }
        
        return Task.CompletedTask;
    }

    public Task RemoveSubscription(PaySubscriptionDto dto)
    {
        var record = _records.SingleOrDefault(record => record.IsEqualTo(dto));
        _records.Remove(record);
        
        _logger.LogInformation($"Stop subscription: {_records}");
        return StopAsync();
    }
}