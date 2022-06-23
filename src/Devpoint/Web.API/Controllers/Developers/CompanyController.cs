using Data.Core;
using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Content.Posts;
using Services.Developers.Companies;
using Services.Subscriptions.Follows;
using Services.Subscriptions.Subscriptions;
using Web.API.Controllers.Content.DTOs;
using Web.API.Controllers.Developers.DTOs;

namespace Web.API.Controllers.Developers;

[ApiController]
[Route("companies")]
public class CompanyController : Controller
{
    private readonly ApplicationContext _context;
    private readonly ICompanyService _companyService;
    private readonly IPostService _postService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IFollowService _followService;

    public CompanyController(
        ICompanyService companyService, 
        IPostService postService, 
        ISubscriptionService subscriptionService, 
        IFollowService followService, 
        ApplicationContext context)
    {
        _companyService = companyService;
        _postService = postService;
        _subscriptionService = subscriptionService;
        _followService = followService;
        _context = context;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllCompanies(string? search = null, int take = 10, int skip = 0, bool isFollow = false)
    {
        var devId = User.GetDevId();
        var query = _companyService.GetAllCompanies();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(e => e.Name.ToLower().Contains(search.ToLower()));
        var totalCount = 0;
        query = query.Include(e => e.Tags);
        List<CompanyDto> result;
        if (devId != null)
        {
            var devIdVal = devId.Value;
            var companies =
                from entity in query
                join follow in _context.Follows
                    on new { eId = entity.Id, dId = devIdVal, type = EntityType.Company }
                    equals new { eId = follow.Target, dId = follow.FollowerId, type = follow.EntityType }
                    into follows
                from ef in follows.DefaultIfEmpty()
                join sub in _context.Subscriptions
                    on new { eId = entity.Id, dId = devId.Value, type = EntityType.Company }
                    equals new { eId = sub.TargetId, dId = sub.SubscriberId, type = sub.EntityType }
                    into subs
                from es in subs.DefaultIfEmpty()
                select new
                {
                    entity,
                    isFollowing = ef != null,
                    userSubscriptionLevel = es == null ? 1 : es.Tariff.SubscriptionLevelId
                };

            if (isFollow)
                companies = companies.Where(o => o.isFollowing);

            totalCount = await companies.CountAsync();
            companies = companies.OrderBy(o => o.entity.Id).Skip(skip).Take(take);

            result = (await companies.ToListAsync())
            .Select(o => new CompanyDto(o.entity)
            {
                IsFollowing = o.isFollowing,
                UserSubscriptionLevel = o.userSubscriptionLevel
            }).ToList();
        }
        else { 
            totalCount = await query.CountAsync();
            query = query.OrderBy(o => o.Id).Skip(skip).Take(take);
            result = (await query.ToListAsync())
            .Select(c => new CompanyDto(c)).ToList();
        }

        return Json(
            new
            {
                totalCount,
                result
            });
    }

    [HttpGet]
    [Route("names")]
    public async Task<IActionResult> GetCompanyNames()
    {
        var names = await _companyService.GetAllCompanies()
            .Select(e => e.Name)
            .ToListAsync();

        return Json(names);
    }

    [HttpGet]
    [Route("{companyIds}")]
    public async Task<IActionResult> GetCompanies(List<Guid> companyIds)
    {
        var companies = await _companyService.GetCompanies(companyIds);
        var result = companies.Select(c => new CompanyDto(c)).ToList();
        
        return Json(result);
    }

    [HttpGet]
    [Route("{companyId:guid}")]
    public async Task<IActionResult> GetCompany(Guid companyId)
    {
        var company = await _companyService.GetCompany(companyId);
        if (company is null)
            return NotFound();
        var result = new CompanyDto(company);
        
        var devId = User.GetDevId();
        if (devId is not null)
        {
            result.IsFollowing = await _followService.IsFollowing(devId.Value, companyId, EntityType.Company);
            result.UserSubscriptionLevel = await _subscriptionService.UserSubscriptionLevel(devId.Value, companyId, EntityType.Company);
        }

        return Json(result);
    }

    [HttpGet]
    [Route("{companyId:guid}/owner")]
    public async Task<IActionResult> GetOwner(Guid companyId)
    {
        var owner = await _companyService.GetOwner(companyId);
        var result = new DeveloperDto(owner);
        
        return Json(result);
    }
    
    [HttpGet]
    [Route("{companyId:guid}/developers")]
    public async Task<IActionResult> GetCompanyDevelopers(Guid companyId)
    {
        var developers = await _companyService.GetCompanyDevelopers(companyId);
        var result = developers.Select(d => new DeveloperDto(d)).ToList();

        return Json(result);
    }
    
    [HttpGet]
    [Route("{companyId:guid}/projects")]
    public async Task<IActionResult> GetCompanyProjects(Guid companyId)
    {
        var projects = await _companyService.GetCompanyProjects(companyId);
        var result = projects.Select(p => new ProjectDto(p)).ToList();
        
        return Json(result);
    }
    
    [HttpGet]
    [Route("{companyId:guid}/tags")]
    public async Task<IActionResult> GetCompanyTags(Guid companyId)
    {
        var tags = await _companyService.GetCompanyTags(companyId);
        var result = tags.Select(t => new TagDto(t)).ToList();
        
        return Json(result);
    }

    [HttpGet]
    [Route("{companyId:guid}/posts")]
    public async Task<IActionResult> GetCompanyPosts(Guid companyId, string? search = null, int take = 3, int skip = 0)
    {
        var devId = User.GetDevId();
        var subLevel = await _subscriptionService.UserCompanySubscriptionLevel(devId, companyId);
        var postsQuery = _postService.GetCompanyPosts(companyId);
        if (!string.IsNullOrEmpty(search))
        {
            var searchString = search.ToLower();
            postsQuery = postsQuery.Where(post => post.Title.ToLower().Contains(searchString));
        }

        var totalCount = await postsQuery.CountAsync();
        postsQuery = postsQuery.OrderByDescending(post => post.DateTime).Skip(skip).Take(take);
        var posts = (await postsQuery
                .ToListAsync())
                .Select(p => 
                    new PostDto(p, _subscriptionService.HasSufficientSubscriptionLevel(p, devId, subLevel)))
                .ToList();

        return Json(new
        {
            totalCount,
            posts
        });
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateCompany(CreateCompanyDto createCompanyDto)
    {
        var companyId = await _companyService.CreateCompany(createCompanyDto.Name, createCompanyDto.Description, createCompanyDto.OwnerId,
            createCompanyDto.Tags.Select(tag =>
            {
                var res = new Tag(tag.Text);
                if (tag.Id != null)
                    res.Id = tag.Id.Value;
                return res;
            }).ToList());
        var tariff = await _context.Tariffs.FirstOrDefaultAsync(t =>
            t.SubscriptionLevelId == 6 && t.SubscriptionType == EntityType.Company);
        if (tariff == null)
            throw new Exception("Tariff not found");
        var sub = await _subscriptionService.CreateSubscription(DateTime.MaxValue, true, 
            tariff.Id, createCompanyDto.OwnerId,
            companyId, EntityType.Company);

        return Ok(companyId);
    }
    
    [HttpPost]
    [Route("{companyId:guid}/switchFollow")]
    public async Task<IActionResult> SwitchFollow(Guid companyId)
    {
        var devId = User.GetDevId();
        if (devId is null)
            return Unauthorized();

        await _followService.SwitchFollowCompany(devId.Value, companyId);
        return Ok();
    }

    [HttpPut]
    [Route("{companyId:guid}/update")]
    public async Task<IActionResult> Update(Guid companyId, UpdateEntityDto updateEntityDto)
    {
        var company = await _companyService.GetCompany(companyId);
        if (company == null)
            return NotFound();
        
        var devId = User.GetDevId();
        if (devId == null)
            return Unauthorized("Unauthorized");

        if (company.OwnerId != devId.Value)
            return StatusCode(403, "Forbidden! Only owner can modify project information");
        
        if (updateEntityDto.Name != null)
            company.Name = updateEntityDto.Name;
        
        if (updateEntityDto.ImagePath != null)
        {
            if (!string.IsNullOrWhiteSpace(company.ImageFullPath))
            {
                var pathToRemove = Path.Combine(Directory.GetCurrentDirectory(), "Resourcess");
                var fullPath = Path.Combine(pathToRemove, company.ImageFullPath);
                System.IO.File.Delete(fullPath);
            }

            company.ImageFullPath = updateEntityDto.ImagePath;
        }

        if (updateEntityDto.Description != null)
            company.Description = updateEntityDto.Description;
        
        await _context.SaveChangesAsync();
        
        if (updateEntityDto.Tags != null)
            await _companyService.UpdateTags(companyId, updateEntityDto.Tags.Select(tag =>
            {
                var res = new Tag(tag.Text);
                if (tag.Id != null)
                    res.Id = tag.Id.Value;
                return res;
            }).ToList());

        return Ok();
    }
    
    [HttpPut]
    [Route("{companyId:guid}/update/coordinates")]
    public async Task<IActionResult> UpdateCoordinates(Guid companyId, decimal latitude, decimal longitude)
    {
        return BadRequest();
        await _companyService.UpdateCoordinates(companyId, latitude, longitude);

        return Ok();
    }
    
    [HttpPut]
    [Route("{companyId:guid}/update/developers")]
    public async Task<IActionResult> UpdateDevelopers(Guid companyId, List<Guid> developerIds)
    {
        var company = await _companyService.GetCompany(companyId);
        if (company == null)
            return NotFound();
        
        var cDevId = User.GetDevId();
        if (cDevId == null)
            return Unauthorized("Unauthorized");

        if (company.OwnerId != cDevId.Value)
            return StatusCode(403, "Forbidden! Only owner can modify project information");
        
        await _companyService.UpdateDevelopers(companyId, developerIds);

        var subsToDelete = await _subscriptionService
            .GetCompanySubscriptions(companyId)
            .Where(s => 
                !developerIds.Contains(s.SubscriberId)
                && s.Tariff.SubscriptionLevelId == 5)
            .ToListAsync();

        _context.RemoveRange(subsToDelete);
        await _context.SaveChangesAsync();
        
        var subs = await _subscriptionService
            .GetCompanySubscriptions(companyId)
            .Where(s => developerIds.Contains(s.SubscriberId))
            .Include(s => s.Tariff)
            .ToListAsync();
        var devSubs = subs.Join(developerIds, sub => sub.SubscriberId, id => id,
                (sub, id) => KeyValuePair.Create(id, sub))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        
        foreach (var devId in developerIds)
        {
            if (devSubs.ContainsKey(devId) && devSubs[devId].Tariff.SubscriptionLevelId >= 5)
                continue;
            if (devSubs.ContainsKey(devId))
            {
                _context.Remove(devSubs[devId]);
                await _context.SaveChangesAsync();
            }

            var tariff = await _context.Tariffs.FirstOrDefaultAsync(t =>
                t.SubscriptionLevelId == 5 && t.SubscriptionType == EntityType.Company);
            if (tariff == null)
                throw new Exception("Tariff not found");
            var sub = await _subscriptionService.CreateSubscription(DateTime.MaxValue, true, 
                tariff.Id, devId,
                companyId, EntityType.Company);
        }

        return Ok();
    }
    
    [HttpPut]
    [Route("{companyId:guid}/update/projects")]
    public async Task<IActionResult> UpdateProjects(Guid companyId, List<Guid> projectIds)
    {
        return BadRequest();
        await _companyService.UpdateProjects(companyId, projectIds);

        return Ok();
    }
}