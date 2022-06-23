namespace Background.Jobs.PaySubscription;

public class SubscriptionRecord
{
    public int SubscriptionId { get; set; }

    public override string ToString()
    {
        return SubscriptionId.ToString();
    }
}