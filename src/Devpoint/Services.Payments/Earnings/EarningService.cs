using Data.Core;
using Domain.Developers.Entities;
using Domain.Payments.Entities;
using Domain.Subscriptions.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Services.Developers.Companies;
using Services.Developers.Projects;
using Services.Payments.Bills;
using Services.Payments.Wallets;
using Services.Subscriptions.Subscriptions;

namespace Services.Payments.Earnings;

public class EarningService : IEarningService
{
    private readonly ApplicationContext _context;
    private readonly IWalletService _walletService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IProjectService _projectService;
    private readonly ICompanyService _companyService;

    public EarningService(ApplicationContext context, 
        IWalletService walletService, 
        ISubscriptionService subscriptionService, 
        ICompanyService companyService, IProjectService projectService)
    {
        _context = context;
        _walletService = walletService;
        _subscriptionService = subscriptionService;
        _companyService = companyService;
        _projectService = projectService;
    }

    public IQueryable<Earning> GetAllEarnings()
    {
        var earnings = _context.Earnings;

        return earnings;
    }

    public async Task<Earning> GetEarning(int earningId)
    {
        var earning = await _context.Earnings.FindAsync(earningId);

        return earning;
    }

    public async Task<Earning> CreateEarning(int walletFromId, int subscriptionId)
    {
        var walletFrom = await _walletService.GetWallet(walletFromId);
        var subscription = await _subscriptionService.GetSubscription(subscriptionId);
        return await CreateEarning(walletFrom, subscription);
    }

    public async Task<Earning> CreateEarning(Wallet walletFrom, Subscription subscription)
    {
        await _context.Entry(subscription).Reference(sub => sub.Tariff).LoadAsync();
        var amount = subscription.Tariff.PricePerMonth;
        if (amount <= 0)
            return null;

        Earning earning;
        if (walletFrom.Amount < amount)
        {
            return null;
        }
        else
        {
            Wallet walletTo;
            switch (subscription.EntityType)
            {
                case EntityType.Developer:
                    walletTo = await _walletService.GetDeveloperWallet(subscription.TargetId);
                    break;
                case EntityType.Project:
                    var project = await _projectService.GetProject(subscription.TargetId);
                    walletTo = await _walletService.GetDeveloperWallet(project.OwnerId);
                    break;
                case EntityType.Company:
                    var company = await _companyService.GetCompany(subscription.TargetId);
                    walletTo = await _walletService.GetDeveloperWallet(company.OwnerId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            earning = new Earning(amount, walletTo, walletFrom, subscription.Tariff, PaymentStatus.Success);
            _context.Earnings.Add(earning);
            walletTo.Amount += amount;
        }

        await _context.SaveChangesAsync();

        return earning;
    }
}