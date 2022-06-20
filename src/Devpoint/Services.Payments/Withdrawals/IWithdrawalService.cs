using Domain.Payments.Entities;

namespace Services.Payments.Withdrawals;

public interface IWithdrawalService
{
    public IQueryable<Withdrawal> GetAllWithdrawals();

    public Task<List<Withdrawal>> GetWithdrawals(List<int> withdrawalIds);

    public Task<Withdrawal> GetWithdrawal(int withdrawalId);

    public Task<Wallet> GetWithdrawalWallet(int withdrawalId);

    public Task<Withdrawal> CreateWithdrawal(double amount, int walletId);
}