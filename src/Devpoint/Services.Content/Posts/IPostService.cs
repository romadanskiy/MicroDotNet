using Domain.Content.Entities;
using Domain.Developers.Entities;
using Domain.Subscriptions.Entities;

namespace Services.Content.Posts;

public interface IPostService
{
    public Task<List<Post>> GetAllPosts();

    public Task<List<Post>> GetPosts(List<int> postIds);

    public IQueryable<Post> GetDeveloperPosts(Guid developerId);
    public IQueryable<Post> GetCompanyPosts(Guid companyId);
    public IQueryable<Post> GetProjectPosts(Guid projectId);

    public Task<Post> GetPost(int postId);

    public Task<SubscriptionLevel> GetRequiredSubscriptionLevel(int postId);

    public Task<Developer> GetPostAuthor(int postId);

    public Task<List<Comment>> GetPostComments(int postId);

    public Task<int> CreatePost(string title, string text, int subscriptionLevelId, Guid developerId, EntityType type, Guid ownerId);

    public Task UpdateText(int postId, string text);

    public Task UpdateRequiredSubscriptionLevel(int postId, int subscriptionLevelId);
}