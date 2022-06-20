using Domain.Developers.Entities;

namespace Domain.Payments.Entities;

public class Wallet
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public Developer Developer { get; set; }
    
    public Wallet(Developer developer)
    {
        Amount = 0;
        Developer = developer;
    }
    
    private Wallet() {}
}