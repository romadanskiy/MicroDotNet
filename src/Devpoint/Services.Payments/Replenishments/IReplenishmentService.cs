using Domain.Payments.Entities;

namespace Services.Payments.Replenishments;

public interface IReplenishmentService
{
    public IQueryable<Replenishment> GetAllReplenishments();

    public Task<List<Replenishment>> GetReplenishments(List<int> replenishmentIds);

    public Task<Replenishment> GetReplenishment(int replenishmentId);

    public Task<Wallet> GetReplenishmentWallet(int replenishmentId);

    public Task<Replenishment> CreateReplenishment(double amount, int walletId);
}