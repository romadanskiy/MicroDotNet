using Domain.Developers.Entities;
using Domain.Payments.Entities;
using Web.API.Controllers.Subscriptions.DTOs;

namespace Web.API.Controllers.Payments.DTOs;

public class BillDto
{
    public int Id { get; set; }
    public double Amount { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public PaymentStatus Status { get; set; }
    
    public TariffDto Tariff { get; set; }

    public BillDto(Bill bill)
    {
        Id = bill.Id;
        Amount = bill.Amount;
        DateTime = bill.DateTime;
        Status = bill.Status;
        Tariff = new TariffDto(bill.Tariff);
    }
}