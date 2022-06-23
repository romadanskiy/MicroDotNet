using Domain.Developers.Entities;

namespace Services.Subscriptions.Follows;

public interface IFollowService
{
    public Task SwitchFollowDeveloper(Guid userId, Guid developerId);
    public Task SwitchFollowCompany(Guid userId, Guid companyId);
    public Task SwitchFollowProject(Guid userId, Guid projectId);

    public Task<bool> IsFollowing(Guid userId, Guid entityId, EntityType type);
}