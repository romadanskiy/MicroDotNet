using Domain.Developers.Entities;

namespace Services.Developers.Tags;

public interface ITagService
{
    public Task<List<Tag>> GetAllTags();

    public Task<List<Tag>> GetTags(List<int> tagIds);

    public Task<Tag> GetTag(int tagId);

    public Task<List<Developer>> GetTagDevelopers(int tagId);
    
    public Task<List<Project>> GetTagProjects(int tagId);
    
    public Task<List<Company>> GetTagCompanies(int tagId);

    public Task<int> CreateTag(string text);

    public Task UpdateText(int tagId, string text);
}