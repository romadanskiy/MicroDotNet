using Domain.Subscriptions.Entities;

namespace Domain.Payments.Entities;

public class Earning
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public Wallet WalletTo { get; set; }
    
    public Wallet WalletFrom { get; set; }
    
    public Tariff Tariff { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public PaymentStatus Status { get; set; }
    
    public Earning(double amount, Wallet walletTo, Wallet walletFrom, Tariff tariff, PaymentStatus status)
    {
        Amount = amount;
        WalletTo = walletTo;
        Tariff = tariff;
        Status = status;
        WalletFrom = walletFrom;
        DateTime = DateTime.UtcNow;
    }
    
    private Earning()
    {
    }
}