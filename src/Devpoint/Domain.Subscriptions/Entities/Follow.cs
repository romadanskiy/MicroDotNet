using Domain.Developers.Entities;

namespace Domain.Subscriptions.Entities;

public class Follow
{
    public int Id { get; set; }

    public Guid Target { get; set; }
    public EntityType EntityType { get; set; }
    public Guid FollowerId { get; set; }
    public Developer Follower { get; set; }
}