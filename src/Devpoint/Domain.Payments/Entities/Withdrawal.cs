namespace Domain.Payments.Entities;

public class Withdrawal
{
    public int Id { get; set; }
    public double Amount { get; set; }

    public DateTime DateTime { get; set; }
    public Wallet Wallet { get; set; }
    
    public Withdrawal(double amount, Wallet wallet)
    {
        Amount = amount;
        Wallet = wallet;
        DateTime = DateTime.UtcNow;
    }
    
    private Withdrawal() {}
}