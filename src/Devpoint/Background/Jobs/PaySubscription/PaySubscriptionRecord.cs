namespace Background.Jobs.PaySubscription;

public class PaySubscriptionRecord
{
    public Guid Id { get; set; }
    public Guid SubscriberWalletId { get; set; }
    
    public Guid TargetId { get; set; }
    
    public double Amount { get; set; }

    public PaySubscriptionRecord(PaySubscriptionDto dto)
    {
        Id = Guid.NewGuid();
        SubscriberWalletId = dto.SubscriberWalletId;
        TargetId = dto.TargetId;
        Amount = dto.Amount;
    }

    public bool IsEqualTo(PaySubscriptionDto dto)
    {
        return SubscriberWalletId == dto.SubscriberWalletId &&
               TargetId == dto.TargetId &&
               Math.Abs(Amount - dto.Amount) < 0.001d;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Withdrawal: WalletId {SubscriberWalletId}, TargetId {TargetId}, Amount: {Amount}";
    }
}