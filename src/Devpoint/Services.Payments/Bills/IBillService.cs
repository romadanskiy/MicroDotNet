using Domain.Payments.Entities;
using Domain.Subscriptions.Entities.Subscriptions;

namespace Services.Payments.Bills;

public interface IBillService
{
    public IQueryable<Bill> GetAllBills();

    public Task<List<Bill>> GetBills(List<int> billIds);

    public Task<Bill> GetBill(int billId);

    public Task<Wallet> GetBillWallet(int billId);

    public Task<Bill> CreateBill(int walletId, int subscriptionId);
    public Task<Bill> CreateBill(Wallet wallet, Subscription subscription);
}