using System.Linq.Expressions;
using Data.Core;
using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Developers.Companies;
using Services.Developers.Developers;
using Services.Developers.Projects;
using Services.Subscriptions.Tariffs;

namespace Services.Subscriptions.Follows;

public class FollowService : IFollowService
{
    private readonly ApplicationContext _context;
    private readonly IDeveloperService _developerService;
    private readonly IProjectService _projectService;
    private readonly ICompanyService _companyService;

    public FollowService(ApplicationContext context, IDeveloperService developerService, IProjectService projectService, ICompanyService companyService)
    {
        _context = context;
        _developerService = developerService;
        _projectService = projectService;
        _companyService = companyService;
    }
    
    public async Task SwitchFollowDeveloper(Guid userId, Guid developerId)
    {
        var follow = await _context.Follows.FirstOrDefaultAsync(follow => follow.FollowerId == userId &&
                                                                      follow.EntityType == EntityType.Developer &&
                                                                      follow.Target == developerId);
        var developer = await _developerService.GetDeveloper(developerId);
        if (follow is not null)
        {
            _context.Follows.Remove(follow);
            developer.SubscriberCount--;
        }
        else
        {
            follow = new Follow()
            {
                Target = developerId,
                EntityType = EntityType.Developer,
                FollowerId = userId
            };
            _context.Follows.Add(follow);
            developer.SubscriberCount++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task SwitchFollowCompany(Guid userId, Guid companyId)
    {
        var follow = await _context.Follows.FirstOrDefaultAsync(follow => follow.FollowerId == userId &&
                                                                          follow.EntityType == EntityType.Company &&
                                                                          follow.Target == companyId);
        var company = await _companyService.GetCompany(companyId);
        if (follow is not null)
        {
            _context.Follows.Remove(follow);
            company.SubscriberCount--;
        }
        else
        {
            follow = new Follow()
            {
                Target = companyId,
                EntityType = EntityType.Company,
                FollowerId = userId
            };
            _context.Follows.Add(follow);
            company.SubscriberCount++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task SwitchFollowProject(Guid userId, Guid projectId)
    {
        var follow = await _context.Follows.FirstOrDefaultAsync(follow => follow.FollowerId == userId &&
                                                                          follow.EntityType == EntityType.Project &&
                                                                          follow.Target == projectId);
        var project = await _projectService.GetProject(projectId);
        if (follow is not null)
        {
            _context.Follows.Remove(follow);
            project.SubscriberCount--;
        }
        else
        {
            follow = new Follow()
            {
                Target = projectId,
                EntityType = EntityType.Project,
                FollowerId = userId
            };
            _context.Follows.Add(follow);
            project.SubscriberCount++;
        }

        await _context.SaveChangesAsync();
    }

    public Task<bool> IsFollowing(Guid userId, Guid entityId, EntityType type)
    {
        return _context.Follows.AnyAsync(follow => follow.FollowerId == userId &&
                                                   follow.EntityType == type &&
                                                   follow.Target == entityId);
    }
}