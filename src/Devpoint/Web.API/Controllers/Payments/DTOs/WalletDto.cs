using Domain.Payments.Entities;

namespace Web.API.Controllers.Payments.DTOs;

public class WalletDto
{
    public int Id { get; set; }
    public double Amount { get; set; }

    public WalletDto(Wallet wallet)
    {
        Id = wallet.Id;
        Amount = wallet.Amount;
    }
}