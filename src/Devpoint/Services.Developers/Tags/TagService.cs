using Data.Core;
using Domain.Developers.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services.Developers.Tags;

public class TagService : ITagService
{
    private readonly ApplicationContext _context;

    public TagService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetAllTags()
    {
        var tags = await _context.Tags.ToListAsync();

        return tags;
    }

    public async Task<List<Tag>> GetTags(List<int> tagIds)
    {
        var tags = await _context.Tags.Where(tag => tagIds.Contains(tag.Id)).ToListAsync();

        return tags;
    }

    public async Task<Tag> GetTag(int tagId)
    {
        var tag = await _context.Tags.FindAsync(tagId);

        return tag;
    }

    public async Task<List<Developer>> GetTagDevelopers(int tagId)
    {
        var tag = await GetTag(tagId);
        await _context.Entry(tag).Collection(t => t.Developers).LoadAsync();

        return tag.Developers;
    }

    public async Task<List<Project>> GetTagProjects(int tagId)
    {
        var tag = await GetTag(tagId);
        await _context.Entry(tag).Collection(t => t.Projects).LoadAsync();

        return tag.Projects;
    }

    public async Task<List<Company>> GetTagCompanies(int tagId)
    {
        var tag = await GetTag(tagId);
        await _context.Entry(tag).Collection(t => t.Companies).LoadAsync();

        return tag.Companies;
    }

    public async Task<int> CreateTag(string text)
    {
        var tag = new Tag(text);
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        return tag.Id;
    }

    public async Task UpdateText(int tagId, string text)
    {
        var tag = await GetTag(tagId);
        tag.Text = text;
        await _context.SaveChangesAsync();
    }
}