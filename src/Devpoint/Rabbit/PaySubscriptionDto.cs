namespace Rabbit;

public class PaySubscriptionDto
{
    public Guid SubscriberWalletId { get; set; }
    
    public Guid TargetId { get; set; }
    
    public double Amount { get; set; }
}