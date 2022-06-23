using Domain.Payments.Entities;

namespace Web.API.Controllers.Payments.DTOs;

public class ReplenishmentDto
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public DateTime DateTime { get; set; }

    public ReplenishmentDto(Replenishment replenishment)
    {
        Id = replenishment.Id;
        Amount = replenishment.Amount;
        DateTime = replenishment.DateTime;
    }
}