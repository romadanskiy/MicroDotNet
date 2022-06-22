using Domain.Content.Entities;
using Domain.Developers.Entities;

namespace Services.Developers.Projects;

public interface IProjectService
{
    public IQueryable<Project> GetAllProjects();

    public Task<List<Project>> GetProjects(List<Guid> projectIds);

    public Task<Project> GetProject(Guid projectId);

    public Task<Developer> GetOwner(Guid projectId);

    public Task<Company> GetProjectCompany(Guid projectId);

    public Task<List<Developer>> GetProjectDevelopers(Guid projectId);

    public Task<List<Tag>> GetProjectTags(Guid projectId);

    public Task<Guid> CreateProject(string name, string description, Guid ownerId, List<Tag> tags, Guid? companyId);

    public Task UpdateName(Guid projectId, string name);

    public Task UpdateDevelopers(Guid projectId, List<Guid> developerIds);

    public Task UpdateTags(Guid projectId, List<Tag> tagIds);
}