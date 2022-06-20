using Domain.Developers.Entities;

namespace Web.API.Controllers.Developers.DTOs;

public class DeveloperDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; }

    public string ImagePath { get; set; }
    
    public int SubscriberCount { get; set; }
    public List<TagDto> Tags { get; set; }
    
    public bool IsFollowing { get; set; } = false;
    public int UserSubscriptionLevel { get; set; } = 0;

    public DeveloperDto(Developer developer)
    {
        Id = developer.Id;
        Name = developer.Name;
        Description = developer.Description;
        ImagePath = developer.ImagePath;
        SubscriberCount = developer.SubscriberCount;
        if (developer.Tags is not null)
            Tags = developer.Tags.Select(tag => new TagDto(tag)).ToList();
    }
    
    public DeveloperDto() {}
}