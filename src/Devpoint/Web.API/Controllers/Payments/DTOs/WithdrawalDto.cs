using Domain.Payments.Entities;

namespace Web.API.Controllers.Payments.DTOs;

public class WithdrawalDto
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public DateTime DateTime { get; set; }

    public WithdrawalDto(Withdrawal withdrawal)
    {
        Id = withdrawal.Id;
        Amount = withdrawal.Amount;
        DateTime = withdrawal.DateTime;
    }
}