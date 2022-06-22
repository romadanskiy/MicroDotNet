using Domain.Developers.Entities;

namespace Web.API.Controllers.Developers.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; }

    public string ImagePath { get; set; }
    public int SubscriberCount { get; set; }

    public Guid OwnerId { get; set; }
    
    public Guid? CompanyId { get; set; }

    public List<TagDto> Tags { get; set; }

    public bool IsFollowing { get; set; } = false;
    public int UserSubscriptionLevel { get; set; } = 0;

    public ProjectDto(Project project)
    {
        Id = project.Id;
        Name = project.Name;
        Description = project.Description;
        ImagePath = project.ImagePath;
        SubscriberCount = project.SubscriberCount;
        OwnerId = project.OwnerId;
        CompanyId = project.CompanyId;
        if (project.Tags is not null)
            Tags = project.Tags.Select(tag => new TagDto(tag)).ToList();
    }
    
    public ProjectDto() {}
}