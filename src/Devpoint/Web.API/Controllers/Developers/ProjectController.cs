using Data.Core;
using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Content.Posts;
using Services.Developers.Projects;
using Services.Subscriptions.Follows;
using Services.Subscriptions.Subscriptions;
using Web.API.Controllers.Content.DTOs;
using Web.API.Controllers.Developers.DTOs;

namespace Web.API.Controllers.Developers;

[ApiController]
[Route("projects")]
public class ProjectController : Controller
{
    private readonly ApplicationContext _context;
    private readonly IProjectService _projectService;
    private readonly IPostService _postService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IFollowService _followService;

    public ProjectController(
        IProjectService projectService, 
        IPostService postService,
        ISubscriptionService subscriptionService, 
        IFollowService followService, 
        ApplicationContext context)
    {
        _projectService = projectService;
        _postService = postService;
        _subscriptionService = subscriptionService;
        _followService = followService;
        _context = context;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllProjects(string? search = null, int take = 10, int skip = 0, bool isFollow = false)
    {
        var devId = User.GetDevId();
        var query = _projectService.GetAllProjects();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(e => e.Name.ToLower().Contains(search.ToLower()));
        
        var totalCount = 0;
        query = query.Include(e => e.Tags);
        List<ProjectDto> result;
        if (devId != null)
        {
            var devIdVal = devId.Value;
            var projects =
                from entity in query
                join follow in _context.Follows
                    on new { eId = entity.Id, dId = devId.Value, type = EntityType.Project }
                    equals new { eId = follow.Target, dId = follow.FollowerId, type = follow.EntityType }
                    into follows
                from ef in follows.DefaultIfEmpty()
                join sub in _context.Subscriptions
                    on new { eId = entity.Id, dId = devIdVal, type = EntityType.Project }
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
                projects = projects.Where(o => o.isFollowing);

            totalCount = await projects.CountAsync();
            projects = projects.OrderBy(o => o.entity.Id).Skip(skip).Take(take);
            
            result = (await projects.ToListAsync())
                .Select(o => new ProjectDto(o.entity)
                {
                    IsFollowing = o.isFollowing,
                    UserSubscriptionLevel = o.userSubscriptionLevel
                }).ToList();
        }
        else {
            totalCount = await query.CountAsync();
            query = query.OrderBy(o => o.Id).Skip(skip).Take(take);
            result = (await query.ToListAsync())
            .Select(c => new ProjectDto(c)).ToList();
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
    public async Task<IActionResult> GetProjectNames()
    {
        var names = await _projectService.GetAllProjects()
            .Select(e => e.Name)
            .ToListAsync();

        return Json(names);
    }

    [HttpGet]
    [Route("{projectIds}")]
    public async Task<IActionResult> GetProjects(List<Guid> projectIds)
    {
        var projects = await _projectService.GetProjects(projectIds);
        var result = projects.Select(p => new ProjectDto(p)).ToList();
        
        return Json(result);
    }

    [HttpGet]
    [Route("{projectId:guid}")]
    public async Task<IActionResult> GetProject(Guid projectId)
    {
        var project = await _projectService.GetProject(projectId);
        if (project is null)
            return NotFound();
        var result = new ProjectDto(project);
        
        var devId = User.GetDevId();
        if (devId is not null)
        {
            result.IsFollowing = await _followService.IsFollowing(devId.Value, projectId, EntityType.Project);
            result.UserSubscriptionLevel = await _subscriptionService.UserSubscriptionLevel(devId.Value, projectId, EntityType.Project);
        }

        return Json(result);
    }

    [HttpGet]
    [Route("{projectId:guid}/owner")]
    public async Task<IActionResult> GetOwner(Guid projectId)
    {
        var owner = await _projectService.GetOwner(projectId);
        var result = new DeveloperDto(owner);

        return Json(result);
    }

    [HttpGet]
    [Route("{projectId:guid}/company")]
    public async Task<IActionResult> GetProjectCompany(Guid projectId)
    {
        var company = await _projectService.GetProjectCompany(projectId);
        var result = new CompanyDto(company);
        
        return Json(result);
    }

    [HttpGet]
    [Route("{projectId:guid}/developers")]
    public async Task<IActionResult> GetProjectDevelopers(Guid projectId)
    {
        var developers = await _projectService.GetProjectDevelopers(projectId);
        var result = developers.Select(d => new DeveloperDto(d)).ToList();
        
        return Json(result);
    }

    [HttpGet]
    [Route("{projectId:guid}/tags")]
    public async Task<IActionResult> GetProjectTags(Guid projectId)
    {
        var tags = await _projectService.GetProjectTags(projectId);
        var result = tags.Select(t => new TagDto(t)).ToList();

        return Json(result);
    }
    
    [HttpGet]
    [Route("{projectId:guid}/posts")]
    public async Task<IActionResult> GetProjectPosts(Guid projectId, string? search = null, int take = 3, int skip = 0)
    {
        var devId = User.GetDevId();
        var subLevel = await _subscriptionService.UserProjectSubscriptionLevel(devId, projectId);
        var postsQuery = _postService.GetProjectPosts(projectId);
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
    public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
    {
        var projectId = await _projectService.CreateProject(createProjectDto.Name, createProjectDto.Description, createProjectDto.OwnerId,
            createProjectDto.Tags.Select(tag =>
            {
                var res = new Tag(tag.Text);
                if (tag.Id != null)
                    res.Id = tag.Id.Value;
                return res;
            }).ToList(), createProjectDto.CompanyId);
        var tariff = await _context.Tariffs.FirstOrDefaultAsync(t =>
            t.SubscriptionLevelId == 6 && t.SubscriptionType == EntityType.Project);
        if (tariff == null)
            throw new Exception("Tariff not found");
        var sub = await _subscriptionService.CreateSubscription(DateTime.MaxValue, true, 
            tariff.Id, createProjectDto.OwnerId,
            projectId, EntityType.Project);

        return Ok(projectId);
    }
    
    [HttpPost]
    [Route("{projectId:guid}/switchFollow")]
    public async Task<IActionResult> SwitchFollow(Guid projectId)
    {
        var devId = User.GetDevId();
        if (devId is null)
            return Unauthorized();

        await _followService.SwitchFollowProject(devId.Value, projectId);
        return Ok();
    }

    [HttpPut]
    [Route("{projectId:guid}/update")]
    public async Task<IActionResult> Update(Guid projectId, [FromBody] UpdateEntityDto updateEntityDto)
    {
        var project = await _projectService.GetProject(projectId);
        if (project == null)
            return NotFound();
        
        var cDevId = User.GetDevId();
        if (cDevId == null)
            return Unauthorized("Unauthorized");

        if (project.OwnerId != cDevId.Value)
            return StatusCode(403, "Forbidden! Only owner can modify project information");
        
        if (updateEntityDto.Name != null)
            project.Name = updateEntityDto.Name;

        if (updateEntityDto.ImagePath != null)
        {
            if (!string.IsNullOrWhiteSpace(project.ImagePath))
            {
                var pathToRemove = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                var fullPath = Path.Combine(pathToRemove, project.ImagePath);
                System.IO.File.Delete(fullPath);
            }

            project.ImagePath = updateEntityDto.ImagePath;
        }

        if (updateEntityDto.Description != null)
            project.Description = updateEntityDto.Description;

        if (updateEntityDto.Tags != null)
            await _projectService.UpdateTags(projectId, updateEntityDto.Tags.Select(tag =>
            {
                var res = new Tag(tag.Text);
                if (tag.Id != null)
                    res.Id = tag.Id.Value;
                return res;
            }).ToList());

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    [Route("{projectId:guid}/update/developers")]
    public async Task<IActionResult> UpdateDevelopers(Guid projectId, List<Guid> developerIds)
    {
        var project = await _projectService.GetProject(projectId);
        if (project == null)
            return NotFound();
        
        var cDevId = User.GetDevId();
        if (cDevId == null)
            return Unauthorized("Unauthorized");

        if (project.OwnerId != cDevId.Value)
            return StatusCode(403, "Forbidden! Only owner can modify project information");
        
        await _projectService.UpdateDevelopers(projectId, developerIds);
        
        var subsToDelete = await _subscriptionService
            .GetProjectSubscriptions(projectId)
            .Where(s => 
                        !developerIds.Contains(s.SubscriberId)
                        && s.Tariff.SubscriptionLevelId == 5)
            .ToListAsync();

        _context.RemoveRange(subsToDelete);
        await _context.SaveChangesAsync();
        
        var subs = await _subscriptionService
                .GetProjectSubscriptions(projectId)
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
                t.SubscriptionLevelId == 5 && t.SubscriptionType == EntityType.Project);
            if (tariff == null)
                throw new Exception("Tariff not found");
            var sub = await _subscriptionService.CreateSubscription(DateTime.MaxValue, true, 
                tariff.Id, devId,
                projectId, EntityType.Project);
        }

        return Ok();
    }
}