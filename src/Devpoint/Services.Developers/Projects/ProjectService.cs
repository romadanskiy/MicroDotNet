using Data.Core;
using Domain.Content.Entities;
using Domain.Developers.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Developers.Tags;

namespace Services.Developers.Projects;

public class ProjectService : IProjectService
{
    private readonly ApplicationContext _context;
    private readonly ITagService _tagService;

    public ProjectService(ApplicationContext context, ITagService tagService)
    {
        _context = context;
        _tagService = tagService;
    }

    public IQueryable<Project> GetAllProjects()
    {
        var projects = _context.Projects;

        return projects;
    }

    public async Task<List<Project>> GetProjects(List<Guid> projectIds)
    {
        var projects = await _context.Projects.Where(project => projectIds.Contains(project.Id)).ToListAsync();

        return projects;
    }

    public async Task<Project> GetProject(Guid projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);

        return project;
    }

    public async Task<Developer> GetOwner(Guid projectId)
    {
        var project = await GetProject(projectId);
        await _context.Entry(project).Reference(p => p.Owner).LoadAsync();

        return project.Owner;
    }

    public async Task<Company> GetProjectCompany(Guid projectId)
    {
        var project = await GetProject(projectId);
        await _context.Entry(project).Reference(p => p.Company).LoadAsync();

        return project.Company;
    }

    public async Task<List<Developer>> GetProjectDevelopers(Guid projectId)
    {
        var project = await GetProject(projectId);
        await _context.Entry(project).Collection(p => p.Developers).LoadAsync();

        return project.Developers;
    }

    public async Task<List<Tag>> GetProjectTags(Guid projectId)
    {
        var project = await GetProject(projectId);
        await _context.Entry(project).Collection(p => p.Tags).LoadAsync();

        return project.Tags;
    }

    public async Task<Guid> CreateProject(string name, string description, Guid ownerId, List<Tag> tags, Guid? companyId)
    {
        var owner = await _context.Developers.FindAsync(ownerId);
        var company = companyId is not null ? await _context.Companies.FindAsync(companyId) : null;
        var project = new Project(name, owner!, company)
        {
            Description = description,
        };
        project.Tags = new List<Tag>();
        var existingTagsIds = tags.Where(tag => tag.Id >= 0).Select(tag => tag.Id).ToList();
        var existingTags = await _context.Tags.Where(tag => existingTagsIds.Contains(tag.Id)).ToListAsync();
        project.Tags.AddRange(existingTags);
        project.Tags.AddRange(tags.Where(tag => tag.Id <= 0).ToList());
        project.Developers = new List<Developer>() { owner };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return project.Id;
    }

    public async Task UpdateName(Guid projectId, string name)
    {
        var project = await GetProject(projectId);
        project.Name = name;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDevelopers(Guid projectId, List<Guid> developerIds)
    {
        var project = await GetProject(projectId);
        await _context.Entry(project).Collection(p => p.Developers).LoadAsync();
        project.Developers.Clear();
        var developers = await _context
            .Developers
            .Where(developer => developerIds.Contains(developer.Id))
            .ToListAsync();
        project.Developers.AddRange(developers);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTags(Guid projectId, List<Tag> tags)
    {
        var project = await GetProject(projectId);
        await _context.Entry(project).Collection(p => p.Tags).LoadAsync();
        if (project.Tags == null)
            project.Tags = new List<Tag>();
        project.Tags.Clear();
        var existingTagsIds = tags.Where(tag => tag.Id >= 0).Select(tag => tag.Id).ToList();
        var existingTags = await _context.Tags.Where(tag => existingTagsIds.Contains(tag.Id)).ToListAsync();
        project.Tags.AddRange(existingTags);
        project.Tags.AddRange(tags.Where(tag => tag.Id <= 0).ToList());
        await _context.SaveChangesAsync();
    }
}