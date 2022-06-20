namespace Web.API.Controllers.Developers.DTOs;

public class CreateCompanyDto
{
    public CreateCompanyDto() {}
    public CreateCompanyDto(string name, string description, Guid ownerId, List<TagDto> tags)
    {
        Name = name;
        Description = description;
        OwnerId = ownerId;
        Tags = tags;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public Guid OwnerId { get; set; }
    public List<TagDto> Tags { get; set; }
}