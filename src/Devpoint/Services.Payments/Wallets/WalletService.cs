using Data.Core;
using Domain.Developers.Entities;
using Domain.Payments.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Developers.Developers;

namespace Services.Payments.Wallets;

public class WalletService : IWalletService
{
    private readonly ApplicationContext _context;
    private readonly IDeveloperService _developerService;

    public WalletService(ApplicationContext context, IDeveloperService developerService)
    {
        _context = context;
        _developerService = developerService;
    }

    public async Task<List<Wallet>> GetAllWallets()
    {
        var wallets = await _context.Wallets.ToListAsync();

        return wallets;
    }

    public async Task<List<Wallet>> GetWallets(List<int> walletIds)
    {
        var wallets = await _context.Wallets.Where(wallet => walletIds.Contains(wallet.Id)).ToListAsync();

        return wallets;
    }

    public async Task<Wallet> GetWallet(int walletId)
    {
        var wallet = await _context.Wallets.FindAsync(walletId);

        return wallet;
    }

    public async Task<Developer> GetWalletDeveloper(int walletId)
    {
        var wallet = await GetWallet(walletId);
        await _context.Entry(wallet).Reference(w => w.Developer).LoadAsync();

        return wallet.Developer;
    }
    
    public async Task<Wallet> GetDeveloperWallet(Guid developerId)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Developer.Id == developerId);

        return wallet;
    }

    public async Task<Wallet> CreateWallet(Guid developerId)
    {
        var developer = await _developerService.GetDeveloper(developerId);
        var wallet = new Wallet(developer);
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();

        return wallet;
    }

    public async Task UpdateAmount(int walletId, double amount)
    {
        var wallet = await GetWallet(walletId);
        wallet.Amount = amount;
        await _context.SaveChangesAsync();
    }
}