using Data.Core;
using Domain.Developers.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Developers.Tags;

namespace Services.Developers.Companies;

public class CompanyService : ICompanyService
{
    private readonly ApplicationContext _context;
    private readonly ITagService _tagService;

    public CompanyService(ApplicationContext context, ITagService tagService)
    {
        _context = context;
        _tagService = tagService;
    }

    public IQueryable<Company> GetAllCompanies()
    {
        var companies = _context.Companies;

        return companies;
    }

    public async Task<List<Company>> GetCompanies(List<Guid> companyIds)
    {
        var companies = await _context.Companies.Where(company => companyIds.Contains(company.Id)).ToListAsync();

        return companies;
    }

    public async Task<Company> GetCompany(Guid companyId)
    {
        var company = await _context.Companies.FindAsync(companyId);

        return company;
    }

    public async Task<Developer> GetOwner(Guid companyId)
    {
        var company = await GetCompany(companyId);
        await _context.Entry(company).Reference(c => c.Owner).LoadAsync();

        return company.Owner;
    }

    public async Task<List<Developer>> GetCompanyDevelopers(Guid companyId)
    {
        var company = await GetCompany(companyId);
        await _context.Entry(company).Collection(c => c.Developers).LoadAsync();

        return company.Developers;
    }

    public async Task<List<Project>> GetCompanyProjects(Guid companyId)
    {
        var company = await GetCompany(companyId);
        await _context.Entry(company).Collection(c => c.Projects).LoadAsync();

        return company.Projects;
    }

    public async Task<List<Tag>> GetCompanyTags(Guid companyId)
    {
        var company = await GetCompany(companyId);
        await _context.Entry(company).Collection(c => c.Tags).LoadAsync();

        return company.Tags;
    }

    public async Task<Guid> CreateCompany(string name, string description, Guid ownerId, List<Tag> tags)
    {
        var owner = await _context.Developers.FindAsync(ownerId);
        
        var company = new Company(name, owner!)
        {
            Description = description,
        };
        company.Tags = new List<Tag>();
        var existingTagsIds = tags.Where(tag => tag.Id >= 0).Select(tag => tag.Id).ToList();
        var existingTags = await _context.Tags.Where(tag => existingTagsIds.Contains(tag.Id)).ToListAsync();
        company.Tags.AddRange(existingTags);
        company.Tags.AddRange(tags.Where(tag => tag.Id <= 0).ToList());
        _context.Companies.Add(company);
        company.Developers = new List<Developer>() { owner };
        await _context.SaveChangesAsync();
        return company.Id;
    }

    public async Task UpdateName(Guid companyId, string name)
    {
        var company = await GetCompany(companyId);
        company.Name = name;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCoordinates(Guid companyId, decimal latitude, decimal longitude)
    {
        var company = await GetCompany(companyId);
        company.Latitude = latitude;
        company.Longitude = longitude;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDevelopers(Guid companyId, List<Guid> developerIds)
    {
        var company = await GetCompany(companyId);
        await _context.Entry(company).Collection(c => c.Developers).LoadAsync();
        company.Developers.Clear();
        var developers = await _context
            .Developers
            .Where(developer => developerIds.Contains(developer.Id))
            .ToListAsync();
        company.Developers.AddRange(developers);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProjects(Guid companyId, List<Guid> projectIds)
    {
        var company = await GetCompany(companyId);
        var projects = await _context
            .Projects
            .Where(project => projectIds.Contains(project.Id))
            .ToListAsync();
        company.Projects = projects;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTags(Guid companyId, List<Tag> tags)
    {
        var company = await GetCompany(companyId);
        await _context.Entry(company).Collection(c => c.Tags).LoadAsync();
        if (company.Tags == null)
            company.Tags = new List<Tag>();
        company.Tags.Clear();
        var existingTagsIds = tags.Where(tag => tag.Id >= 0).Select(tag => tag.Id).ToList();
        var existingTags = await _context.Tags.Where(tag => existingTagsIds.Contains(tag.Id)).ToListAsync();
        company.Tags.AddRange(existingTags);
        company.Tags.AddRange(tags.Where(tag => tag.Id <= 0).ToList());
        await _context.SaveChangesAsync();
    }
}