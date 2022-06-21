using Domain.Payments.Entities;
using Domain.Subscriptions.Entities.Subscriptions;

namespace Services.Payments.Earnings;

public interface IEarningService
{
    public IQueryable<Earning> GetAllEarnings();
    
    public Task<Earning> GetEarning(int earningId);

    public Task<Earning> CreateEarning(int walletFromId, int subscriptionId);
    public Task<Earning> CreateEarning(Wallet walletFrom, Subscription subscription);
}