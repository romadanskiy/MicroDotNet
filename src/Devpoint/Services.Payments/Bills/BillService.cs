using Data.Core;
using Domain.Payments.Entities;
using Domain.Subscriptions.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Services.Payments.Wallets;
using Services.Subscriptions.Subscriptions;

namespace Services.Payments.Bills;

public class BillService : IBillService
{
    private readonly ApplicationContext _context;
    private readonly IWalletService _walletService;
    private readonly ISubscriptionService _subscriptionService;

    public BillService(ApplicationContext context, IWalletService walletService, ISubscriptionService subscriptionService)
    {
        _context = context;
        _walletService = walletService;
        _subscriptionService = subscriptionService;
    }

    public IQueryable<Bill> GetAllBills()
    {
        var bills = _context.Bills;

        return bills;
    }

    public async Task<List<Bill>> GetBills(List<int> billIds)
    {
        var bills = await _context.Bills.Where(bill => billIds.Contains(bill.Id)).ToListAsync();

        return bills;
    }

    public async Task<Bill> GetBill(int billId)
    {
        var bill = await _context.Bills.FindAsync(billId);

        return bill;
    }

    public async Task<Wallet> GetBillWallet(int billId)
    {
        var bill = await GetBill(billId);
        await _context.Entry(bill).Reference(b => b.Wallet).LoadAsync();

        return bill.Wallet;
    }

    public async Task<Bill> CreateBill(int walletId, int subscriptionId)
    {
        var wallet = await _walletService.GetWallet(walletId);
        var subscription = await _subscriptionService.GetSubscription(subscriptionId);
        return await CreateBill(wallet, subscription);
    }

    public async Task<Bill> CreateBill(Wallet wallet, Subscription subscription)
    {
        await _context.Entry(subscription).Reference(sub => sub.Tariff).LoadAsync();
        var amount = subscription.Tariff.PricePerMonth;
        if (amount <= 0)
            return null;

        Bill bill;
        if (wallet.Amount < amount)
        {
            bill = new Bill(amount, wallet, subscription.Tariff, PaymentStatus.Failed);
            _context.Bills.Add(bill);
            _context.Remove(subscription);
        }
        else
        {
            bill = new Bill(amount, wallet, subscription.Tariff, PaymentStatus.Success);
            _context.Bills.Add(bill);
            wallet.Amount -= amount;
        }

        await _context.SaveChangesAsync();

        return bill;
    }
}