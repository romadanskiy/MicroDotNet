namespace Web.API.Controllers.Developers.DTOs;

public class UpdateEntityDto
{
    public UpdateEntityDto() {}
    
    public UpdateEntityDto(string? name = null, string? imagePath = null, string? description = null)
    {
        Name = name;
        ImagePath = imagePath;
        Description = description;
    }

    public string? Name { get; set; }
    public string? ImagePath { get; set; }
    public string? Description { get; set; }
    public List<TagDto>? Tags { get; set; }
}