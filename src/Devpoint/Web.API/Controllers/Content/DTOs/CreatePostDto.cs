using Domain.Developers.Entities;

namespace Web.API.Controllers.Content.DTOs;

public class CreatePostDto
{
    public CreatePostDto(string title, string text, int subscriptionLevelId, Guid developerId, EntityType type, Guid ownerId)
    {
        Title = title;
        Text = text;
        SubscriptionLevelId = subscriptionLevelId;
        DeveloperId = developerId;
        Type = type;
        OwnerId = ownerId;
    }
    
    public CreatePostDto() {}

    public string Title { get; set; }
    public string Text { get; set; }
    public int SubscriptionLevelId { get; set; }
    public Guid DeveloperId { get; set; }
    public EntityType Type { get; set; }
    public Guid OwnerId { get; set; }
}