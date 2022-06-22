using Domain.Developers.Entities;

namespace Services.Developers.Companies;

public interface ICompanyService
{
    public IQueryable<Company> GetAllCompanies();

    public Task<List<Company>> GetCompanies(List<Guid> companyIds);

    public Task<Company> GetCompany(Guid companyId);

    public Task<Developer> GetOwner(Guid companyId);

    public Task<List<Developer>> GetCompanyDevelopers(Guid companyId);

    public Task<List<Project>> GetCompanyProjects(Guid companyId);

    public Task<List<Tag>> GetCompanyTags(Guid companyId);

    public Task<Guid> CreateCompany(string name, string description, Guid ownerId, List<Tag> tags);

    public Task UpdateName(Guid companyId, string name);

    public Task UpdateCoordinates(Guid companyId, decimal latitude, decimal longitude);

    public Task UpdateDevelopers(Guid companyId, List<Guid> developerIds);

    public Task UpdateProjects(Guid companyId, List<Guid> projectIds);

    public Task UpdateTags(Guid companyId, List<Tag> tags);
}