using Domain.Content.Entities;

namespace Web.API.Controllers.Content.DTOs;

public class PostDto
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string Content { get; set; } = "";

    public string ImagePath { get; set; } = "";
    
    public DateTime Date { get; set; } = DateTime.MinValue;

    public int RequiredSubscriptionLevel { get; set; }

    public bool HasUserAccess { get; set; } = false;

    public PostDto(Post post, bool hasUserAccess)
    {
        Id = post.Id;
        HasUserAccess = hasUserAccess;
        RequiredSubscriptionLevel = post.RequiredSubscriptionLevelId;
        if (HasUserAccess)
        {
            Content = post.Content;
            Title = post.Title;
            ImagePath = post.ImagePath;
            Date = post.DateTime;
        }
    }
    
    public PostDto() {}
}