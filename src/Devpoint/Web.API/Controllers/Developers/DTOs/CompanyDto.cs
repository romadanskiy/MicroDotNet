using Domain.Developers.Entities;

namespace Web.API.Controllers.Developers.DTOs;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }

    public string ImagePath { get; set; }
    
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public int SubscriberCount { get; set; }
    public Guid OwnerId { get; set; }
    public List<TagDto> Tags { get; set; }
    
    public bool IsFollowing { get; set; } = false;
    public int UserSubscriptionLevel { get; set; } = 0;

    public CompanyDto(Company company)
    {
        Id = company.Id;
        Name = company.Name;
        Latitude = company.Latitude;
        Longitude = company.Longitude;
        Description = company.Description;
        ImagePath = company.ImageFullPath;
        SubscriberCount = company.SubscriberCount;
        OwnerId = company.OwnerId;
        if (company.Tags is not null)
            Tags = company.Tags.Select(tag => new TagDto(tag)).ToList();
    }
    
    public CompanyDto() {}
}