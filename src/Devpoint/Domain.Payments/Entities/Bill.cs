using Domain.Subscriptions.Entities;
using Domain.Subscriptions.Entities.Subscriptions;

namespace Domain.Payments.Entities;

public class Bill
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public Wallet Wallet { get; set; }
    
    public Tariff Tariff { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public PaymentStatus Status { get; set; }
    
    public Bill(double amount, Wallet wallet, Tariff tariff, PaymentStatus status)
    {
        Amount = amount;
        Wallet = wallet;
        Tariff = tariff;
        Status = status;
        DateTime = DateTime.UtcNow;
    }
    
    private Bill() {}
}