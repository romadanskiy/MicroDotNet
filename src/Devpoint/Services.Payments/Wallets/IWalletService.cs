using Domain.Developers.Entities;
using Domain.Payments.Entities;

namespace Services.Payments.Wallets;

public interface IWalletService
{
    public Task<List<Wallet>> GetAllWallets();

    public Task<List<Wallet>> GetWallets(List<int> walletIds);

    public Task<Wallet> GetWallet(int walletId);

    public Task<Developer> GetWalletDeveloper(int walletId);

    public Task<Wallet> GetDeveloperWallet(Guid developerId);

    public Task<Wallet> CreateWallet(Guid developerId);

    public Task UpdateAmount(int walletId, double amount);
}