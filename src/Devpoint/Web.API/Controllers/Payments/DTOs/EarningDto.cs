using Domain.Developers.Entities;
using Domain.Payments.Entities;
using Web.API.Controllers.Developers.DTOs;
using Web.API.Controllers.Subscriptions.DTOs;

namespace Web.API.Controllers.Payments.DTOs;

public class EarningDto
{
    public int Id { get; set; }
    public double Amount { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public PaymentStatus Status { get; set; }
    
    public TariffDto Tariff { get; set; }
    
    public DeveloperDto Developer { get; set; }

    public EarningDto(Earning earning)
    {
        Id = earning.Id;
        Amount = earning.Amount;
        DateTime = earning.DateTime;
        Status = earning.Status;
        Tariff = new TariffDto(earning.Tariff);
        Developer = new DeveloperDto(earning.WalletFrom.Developer);
    }
}