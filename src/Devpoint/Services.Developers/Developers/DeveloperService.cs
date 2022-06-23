using Data.Core;
using Domain.Developers.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Developers.Tags;

namespace Services.Developers.Developers;

public class DeveloperService : IDeveloperService
{
    private readonly ApplicationContext _context;
    private readonly ITagService _tagService;

    public DeveloperService(ApplicationContext context, ITagService tagService)
    {
        _context = context;
        _tagService = tagService;
    }

    public IQueryable<Developer> GetAllDevelopers()
    {
        var developers = _context.Developers;

        return developers;
    }

    public async Task<List<Developer>> GetDevelopers(List<Guid> developerIds)
    {
        var developers = await _context.Developers.Where(developer => developerIds.Contains(developer.Id)).ToListAsync();

        return developers;
    }

    public async Task<Developer> GetDeveloper(Guid developerId)
    {
        var developer = await _context.Developers.FindAsync(developerId);

        return developer;
    }

    public async Task<List<Project>> GetOwnedProjects(Guid developerId)
    {
        var developer = await GetDeveloper(developerId);
        await _context.Entry(developer).Collection(d => d.OwnedProjects).LoadAsync();

        return developer.OwnedProjects;
    }

    public async Task<List<Project>> GetDeveloperProjects(Guid developerId)
    {
        var developer = await GetDeveloper(developerId);
        await _context.Entry(developer).Collection(d => d.Projects).LoadAsync();

        return developer.Projects;
    }

    public async Task<List<Company>> GetOwnedCompanies(Guid developerId)
    {
        var developer = await GetDeveloper(developerId);
        await _context.Entry(developer).Collection(d => d.OwnedCompanies).LoadAsync();

        return developer.OwnedCompanies;
    }

    public async Task<List<Company>> GetDeveloperCompanies(Guid developerId)
    {
        var developer = await GetDeveloper(developerId);
        await _context.Entry(developer).Collection(d => d.Companies).LoadAsync();

        return developer.Companies;
    }

    public async Task<List<Tag>> GetDeveloperTags(Guid developerId)
    {
        var developer = await GetDeveloper(developerId);
        await _context.Entry(developer).Collection(d => d.Tags).LoadAsync();

        return developer.Tags;
    }

    public async Task<Developer> CreateDeveloper(string name, string description)
    {
        var developer = new Developer(name)
        {
            Description = description
        };
        _context.Developers.Add(developer);
        await _context.SaveChangesAsync();

        return developer;
    }

    public async Task UpdateName(Guid developerId, string name)
    {
        var developer = await GetDeveloper(developerId);
        developer.Name = name;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProjects(Guid developerId, List<Guid> projectIds)
    {
        var developer = await GetDeveloper(developerId);
        var projects = await _context
            .Projects
            .Where(project => projectIds.Contains(project.Id))
            .ToListAsync();
        developer.Projects = projects;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCompanies(Guid developerId, List<Guid> companyIds)
    {
        var developer = await GetDeveloper(developerId);
        var companies = await _context
            .Companies
            .Where(company => companyIds.Contains(company.Id))
            .ToListAsync();
        developer.Companies = companies;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTags(Guid developerId, List<Tag> tags)
    {
        var developer = await GetDeveloper(developerId);
        await _context.Entry(developer).Collection(c => c.Tags).LoadAsync();
        if (developer.Tags == null)
            developer.Tags = new List<Tag>();
        developer.Tags.Clear();
        var existingTagsIds = tags.Where(tag => tag.Id >= 0).Select(tag => tag.Id).ToList();
        var existingTags = await _context.Tags.Where(tag => existingTagsIds.Contains(tag.Id)).ToListAsync();
        developer.Tags.AddRange(existingTags);
        developer.Tags.AddRange(tags.Where(tag => tag.Id <= 0).ToList());
        await _context.SaveChangesAsync();
    }
}