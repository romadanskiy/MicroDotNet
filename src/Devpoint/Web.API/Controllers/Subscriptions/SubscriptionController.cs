using Data.Core;
using Domain.Developers.Entities;
using Domain.Payments.Entities;
using Domain.Subscriptions.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Developers.Companies;
using Services.Developers.Developers;
using Services.Developers.Projects;
using Services.Payments.Bills;
using Services.Payments.Wallets;
using Services.Subscriptions.Subscriptions;
using Services.Subscriptions.Tariffs;
using Web.API.Controllers.Developers.DTOs;
using Web.API.Controllers.Subscriptions.DTOs;

namespace Web.API.Controllers.Subscriptions;

[ApiController]
[Route("subscriptions")]
public class SubscriptionController : Controller
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IBillService _billService;
    private readonly IWalletService _walletService;
    private readonly ITariffService _tariffService;
    private readonly ICompanyService _companyService;
    private readonly IProjectService _projectService;
    private readonly IDeveloperService _developerService;
    private readonly ApplicationContext _context;

    public SubscriptionController(
        ISubscriptionService subscriptionService, 
        IBillService billService, IWalletService walletService, 
        ITariffService tariffService, ICompanyService companyService, 
        IProjectService projectService, IDeveloperService developerService, 
        ApplicationContext context)
    {
        _subscriptionService = subscriptionService;
        _billService = billService;
        _walletService = walletService;
        _tariffService = tariffService;
        _companyService = companyService;
        _projectService = projectService;
        _developerService = developerService;
        _context = context;
    }

    [HttpGet]
    [Route("company")]
    public async Task<IActionResult> GetCompanySubscriptions()
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();

        var query = _subscriptionService
            .GetAllSubscriptions()
            .Where(sub => sub.SubscriberId == devId && 
                                    sub.EntityType == EntityType.Company)
            .Include(sub => sub.Tariff);

        var fullQuery =
            from sub in query
            join company in _companyService.GetAllCompanies()
                on sub.TargetId equals company.Id
            select new { sub, company };

        var subscriptions = await fullQuery.ToListAsync();
        var result = subscriptions
            .Select(o => new SubscriptionDto(o.sub)
            {
                Entity = EntityDto.FromCompany(o.company)
            })
            .ToList();

        return Json(result);
    }
    
    [HttpGet]
    [Route("project")]
    public async Task<IActionResult> GetProjectSubscriptions()
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();

        var query = _subscriptionService
            .GetAllSubscriptions()
            .Where(sub => sub.SubscriberId == devId && 
                          sub.EntityType == EntityType.Project)
            .Include(sub => sub.Tariff);

        var fullQuery =
            from sub in query
            join project in _projectService.GetAllProjects()
                on sub.TargetId equals project.Id
            select new { sub, project };

        var subscriptions = await fullQuery.ToListAsync();
        var result = subscriptions
            .Select(o => new SubscriptionDto(o.sub)
            {
                Entity = EntityDto.FromProject(o.project)
            })
            .ToList();

        return Json(result);
    }
    
    [HttpGet]
    [Route("developer")]
    public async Task<IActionResult> GetDeveloperSubscriptions()
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();

        var query = _subscriptionService
            .GetAllSubscriptions()
            .Where(sub => sub.SubscriberId == devId && 
                          sub.EntityType == EntityType.Developer)
            .Include(sub => sub.Tariff);

        var fullQuery =
            from sub in query
            join developer in _developerService.GetAllDevelopers()
                on sub.TargetId equals developer.Id
            select new { sub, developer };

        var subscriptions = await fullQuery.ToListAsync();
        var result = subscriptions
            .Select(o => new SubscriptionDto(o.sub)
            {
                Entity = EntityDto.FromDeveloper(o.developer)
            })
            .ToList();

        return Json(result);
    }

    [HttpGet]
    [Route("{subscriptionId:int}")]
    public async Task<IActionResult> GetSubscription(int subscriptionId)
    {
        var subscription = await _subscriptionService.GetSubscription(subscriptionId);
        var result = new SubscriptionDto(subscription);

        return Ok(result);
    }
    
    [HttpGet]
    [Route("find")]
    public async Task<IActionResult> GetSubscription(Guid targetId, EntityType type)
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();

        var subscription = await _subscriptionService.FindSubscription(devId.Value, targetId, type);
        if (subscription == null)
            return NotFound();

        EntityDto entityDto;

        switch (subscription.EntityType)
        {
            case EntityType.Developer:
                var developer = await _developerService.GetDeveloper(subscription.TargetId);
                entityDto = EntityDto.FromDeveloper(developer);
                break;
            case EntityType.Project:
                var project = await _projectService.GetProject(subscription.TargetId);
                entityDto = EntityDto.FromProject(project);
                break;
            case EntityType.Company:
                var company = await _companyService.GetCompany(subscription.TargetId);
                entityDto = EntityDto.FromCompany(company);
                break;
            default:
                return BadRequest();
        }

        var result = new SubscriptionDto(subscription)
        {
            Entity = entityDto
        };

        return Json(result);
    }

    [HttpGet]
    [Route("{subscriptionId:int}/tariff")]
    public async Task<IActionResult> GetSubscriptionTariff(int subscriptionId)
    {
        var tariff = await _subscriptionService.GetSubscriptionTariff(subscriptionId);
        var result = new TariffDto(tariff);

        return Ok(result);
    }

    [HttpGet]
    [Route("{subscriptionId:int}/subscriber")]
    public async Task<IActionResult> GetSubscriptionSubscriber(int subscriptionId)
    {
        var subscriber = await _subscriptionService.GetSubscriptionSubscriber(subscriptionId);
        var result = new DeveloperDto(subscriber);

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionDto createSubscriptionDto)
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();

        var tariff =
            await _tariffService.GetTariff(createSubscriptionDto.SubscriptionLevelId, createSubscriptionDto.Type);
        if (tariff == null)
            return BadRequest("Tariff does not exist");

        var existingSubscription = await _subscriptionService.FindSubscription(devId.Value,
            createSubscriptionDto.TargetId, createSubscriptionDto.Type);
        
        var subscription = await _subscriptionService
            .CreateSubscription(
                DateTime.UtcNow.AddMonths(1), createSubscriptionDto.IsAutoRenewal, tariff.Id, 
                devId.Value, createSubscriptionDto.TargetId, createSubscriptionDto.Type);
        
        var wallet = await _walletService.GetDeveloperWallet(devId.Value) ?? 
                     await _walletService.CreateWallet(devId.Value);

        var bill = await _billService.CreateBill(wallet, subscription);
        if (bill.Status == PaymentStatus.Failed)
            return BadRequest("Insufficient Funds");

        if (existingSubscription != null)
        {
            _context.Remove(existingSubscription);
            await _context.SaveChangesAsync();
        }

        return Ok(bill);
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateSubscription([FromBody] UpdateSubscriptionDto updateSubscriptionDto)
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();

        var subscription = await _subscriptionService.GetSubscription(updateSubscriptionDto.SubscriptionId);

        if (subscription == null)
            return NotFound();

        if (subscription.SubscriberId != devId)
            return Forbid();

        subscription.IsAutoRenewal = updateSubscriptionDto.IsAutoRenewable;
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpDelete]
    [Route("{subscriptionId:int}/cancel")]
    public async Task<IActionResult> CancelSubscription(int subscriptionId)
    {
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized();

        var subscription = await _subscriptionService.GetSubscription(subscriptionId);

        if (subscription == null)
            return NotFound();

        if (subscription.SubscriberId != devId)
            return Forbid();

        _context.Remove(subscription);
        await _context.SaveChangesAsync();

        return Ok();
    }
}