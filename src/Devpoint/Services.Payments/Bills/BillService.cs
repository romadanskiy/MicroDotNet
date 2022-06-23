using Data.Core;
using Domain.Developers.Entities;
using Domain.Payments.Entities;
using Domain.Subscriptions.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Services.Payments.Earnings;
using Services.Payments.Rabbit;
using Services.Payments.Wallets;
using Services.Subscriptions.Subscriptions;

namespace Services.Payments.Bills;

public class BillService : IBillService
{
    private readonly ApplicationContext _context;
    private readonly IWalletService _walletService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IEarningService _earningService;
    private readonly IRabbitUnsubscribePublisher _unsubscribePublisher;

    public BillService(
        ApplicationContext context, IWalletService walletService, 
        ISubscriptionService subscriptionService, IEarningService earningService, IRabbitUnsubscribePublisher unsubscribePublisher)
    {
        _context = context;
        _walletService = walletService;
        _subscriptionService = subscriptionService;
        _earningService = earningService;
        _unsubscribePublisher = unsubscribePublisher;
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

    public async Task<Bill> CreateBill(int subscriptionId)
    {
        var subscription = await _subscriptionService.GetSubscription(subscriptionId);
        var wallet = await _walletService.GetDeveloperWallet(subscription.SubscriberId);
        
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

            if (subscription.IsAutoRenewal)
            {
                var subscriptionRecord = new SubscriptionRecord {SubscriptionId = subscription.Id};
                _unsubscribePublisher.SendMessage(subscriptionRecord);
            }
        }
        else
        {
            bill = new Bill(amount, wallet, subscription.Tariff, PaymentStatus.Success);
            _context.Bills.Add(bill);
            wallet.Amount -= amount;

            await _earningService.CreateEarning(wallet, subscription);
        }

        await _context.SaveChangesAsync();

        return bill;
    }
}