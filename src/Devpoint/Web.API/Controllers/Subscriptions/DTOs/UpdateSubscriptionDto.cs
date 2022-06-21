namespace Web.API.Controllers.Subscriptions.DTOs;

public class UpdateSubscriptionDto
{
    public int SubscriptionId { get; set; }
    public bool IsAutoRenewable { get; set; }
}