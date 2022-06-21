using Domain.Developers.Entities;

namespace Web.API.Controllers.Developers.DTOs;

public class TagDto
{
    public int? Id { get; set; } = null;
    public string Text { get; set; }

    public TagDto(Tag tag)
    {
        Id = tag.Id;
        Text = tag.Text;
    }
    
    public TagDto() {}
}