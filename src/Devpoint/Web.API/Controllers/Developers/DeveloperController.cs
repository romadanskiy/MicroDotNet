using Data.Core;
using Domain.Developers.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Content.Posts;
using Services.Developers.Developers;
using Services.Subscriptions.Follows;
using Services.Subscriptions.Subscriptions;
using Web.API.Controllers.Content.DTOs;
using Web.API.Controllers.Developers.DTOs;

namespace Web.API.Controllers.Developers;

[ApiController]
[Route("developers")]
public class DeveloperController : Controller
{
    private readonly ApplicationContext _context;
    private readonly IDeveloperService _developerService;
    private readonly IPostService _postService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IFollowService _followService;

    public DeveloperController(
        IDeveloperService developerService, 
        IPostService postService, 
        ISubscriptionService subscriptionService, 
        IFollowService followService, 
        ApplicationContext context)
    {
        _developerService = developerService;
        _postService = postService;
        _subscriptionService = subscriptionService;
        _followService = followService;
        _context = context;
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> GetAllDevelopers(string? search = null, int take = 10, int skip = 0, bool isFollow = false)
    {
        var devId = User.GetDevId();
        var query = _developerService.GetAllDevelopers();
        
        if (!string.IsNullOrEmpty(search))
            query = query.Where(entity => entity.Name.ToLower().Contains(search.ToLower()));

        var totalCount = 0;
        query = query.Include(e => e.Tags);
        List<DeveloperDto> result;
        if (devId != null)
        {
            var devIdVal = devId.Value;
            var developers =
                from entity in query
                join follow in _context.Follows
                    on new { eId = entity.Id, dId = devId.Value, type = EntityType.Developer }
                    equals new { eId = follow.Target, dId = follow.FollowerId, type = follow.EntityType }
                    into follows
                from ef in follows.DefaultIfEmpty()
                join sub in _context.Subscriptions
                    on new { eId = entity.Id, dId = devIdVal, type = EntityType.Developer }
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
                developers = developers.Where(o => o.isFollowing);

            totalCount = await developers.CountAsync();
            developers = developers.OrderBy(o => o.entity.Id).Skip(skip).Take(take);

            result = (await developers.ToListAsync())
                .Select(o => new DeveloperDto(o.entity)
                {
                    IsFollowing = o.isFollowing,
                    UserSubscriptionLevel = o.userSubscriptionLevel
                }).ToList();
        }
        else {
            totalCount = await query.CountAsync();
            query = query.OrderBy(o => o.Id).Skip(skip).Take(take);
            result = (await query.ToListAsync())
            .Select(c => new DeveloperDto(c)).ToList();
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
    public async Task<IActionResult> GetDeveloperNames()
    {
        var names = await _developerService.GetAllDevelopers()
            .Select(e => e.Name)
            .ToListAsync();

        return Json(names);
    }

    [HttpGet]
    [Route("{developerIds}")]
    public async Task<IActionResult> GetDevelopers(List<Guid> developerIds)
    {
        var developers = await _developerService.GetDevelopers(developerIds);
        var result = developers.Select(d => new DeveloperDto(d));
        
        return Json(result);
    }

    [HttpGet]
    [Route("{developerId:guid}")]
    public async Task<IActionResult> GetDeveloper(Guid developerId)
    {
        var developer = await _developerService.GetDeveloper(developerId);
        if (developer is null)
            return NotFound();
        var result = new DeveloperDto(developer);
        
        var devId = User.GetDevId();
        if (devId is not null)
        {
            result.IsFollowing = await _followService.IsFollowing(devId.Value, developerId, EntityType.Developer);
            result.UserSubscriptionLevel = await _subscriptionService.UserSubscriptionLevel(devId.Value, developerId, EntityType.Developer);
        }

        return Json(result);
    }

    [HttpGet]
    [Route("{developerId:guid}/owned-projects")]
    public async Task<IActionResult> GetOwnedProjects(Guid developerId)
    {
        var projects = await _developerService.GetOwnedProjects(developerId);
        var result = projects.Select(p => new ProjectDto(p)).ToList();

        return Json(result);
    }
    
    [HttpGet]
    [Route("{developerId:guid}/projects")]
    public async Task<IActionResult> GetDeveloperProjects(Guid developerId)
    {
        var projects = await _developerService.GetDeveloperProjects(developerId);
        var result = projects.Select(p => new ProjectDto(p)).ToList();

        return Json(result);
    }
    
    [HttpGet]
    [Route("{developerId:guid}/owned-companies")]
    public async Task<IActionResult> GetOwnedCompanies(Guid developerId)
    {
        var companies = await _developerService.GetOwnedCompanies(developerId);
        var result = companies.Select(c => new CompanyDto(c)).ToList();

        return Json(result);
    }
    
    [HttpGet]
    [Route("{developerId:guid}/companies")]
    public async Task<IActionResult> GetDeveloperCompanies(Guid developerId)
    {
        var companies = await _developerService.GetDeveloperCompanies(developerId);
        var result = companies.Select(c => new CompanyDto(c)).ToList();

        return Json(result);
    }
    
    [HttpGet]
    [Route("{developerId:guid}/tags")]
    public async Task<IActionResult> GetDeveloperTags(Guid developerId)
    {
        var tags = await _developerService.GetDeveloperTags(developerId);
        var result = tags.Select(t => new TagDto(t)).ToList();
        
        return Json(result);
    }
    
    [HttpGet]
    [Route("{developerId:guid}/posts")]
    public async Task<IActionResult> GetDeveloperPosts(Guid developerId, string? search = null, int take = 3, int skip = 0)
    {
        var devId = User.GetDevId();
        var subLevel = await _subscriptionService.UserDeveloperSubscriptionLevel(devId, developerId);
        var postsQuery = _postService.GetDeveloperPosts(developerId);
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
    public async Task<IActionResult> CreateDeveloper(string name, string description)
    {
        var developerId = await _developerService.CreateDeveloper(name, description);

        return Json(developerId);
    }
    
    [HttpPost]
    [Route("{developerId:guid}/switchFollow")]
    public async Task<IActionResult> SwitchFollow(Guid developerId)
    {
        var devId = User.GetDevId();
        if (devId is null)
            return Unauthorized();

        await _followService.SwitchFollowDeveloper(devId.Value, developerId);
        return Ok();
    }
    
    [HttpPut]
    [Route("{developerId:guid}/update")]
    public async Task<IActionResult> UpdateDeveloper(Guid developerId, [FromBody] UpdateEntityDto updateEntityDto)
    {
        var developer = await _developerService.GetDeveloper(developerId);

        if (developer == null)
            return NotFound();
        
        var cDevId = User.GetDevId();
        if (cDevId == null)
            return Unauthorized("Unauthorized");

        if (developer.Id != cDevId.Value)
            return StatusCode(403, "Forbidden! Only owner can modify project information");
        
        if (updateEntityDto.Name != null)
            developer.Name = updateEntityDto.Name;
        
        if (updateEntityDto.ImagePath != null)
        {
            if (!string.IsNullOrWhiteSpace(developer.ImagePath))
            {
                var pathToRemove = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                var fullPath = Path.Combine(pathToRemove, developer.ImagePath);
                System.IO.File.Delete(fullPath);
            }

            developer.ImagePath = updateEntityDto.ImagePath;
        }

        if (updateEntityDto.Description != null)
            developer.Description = updateEntityDto.Description;
        
        await _context.SaveChangesAsync();
        
        if (updateEntityDto.Tags != null)
            await _developerService.UpdateTags(developerId, updateEntityDto.Tags.Select(tag =>
            {
                var res = new Tag(tag.Text);
                if (tag.Id != null)
                    res.Id = tag.Id.Value;
                return res;
            }).ToList());

        return Ok();
    }
    
    [HttpPut]
    [Route("{developerId:guid}/update/projects")]
    public async Task<IActionResult> UpdateProjects(Guid developerId, List<Guid> projectIds)
    {
        return BadRequest();
        await _developerService.UpdateProjects(developerId, projectIds);

        return Ok();
    }
    
    [HttpPut]
    [Route("{developerId:guid}/update/companies")]
    public async Task<IActionResult> UpdateCompanies(Guid developerId, List<Guid> companyIds)
    {
        return BadRequest();
        await _developerService.UpdateCompanies(developerId, companyIds);

        return Ok();
    }
}