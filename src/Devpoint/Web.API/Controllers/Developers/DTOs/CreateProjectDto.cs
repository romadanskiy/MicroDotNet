namespace Web.API.Controllers.Developers.DTOs;

public class CreateProjectDto
{
    public CreateProjectDto(string name, string description, Guid ownerId, List<TagDto> tags, Guid? companyId)
    {
        Name = name;
        Description = description;
        OwnerId = ownerId;
        Tags = tags;
        CompanyId = companyId;
    }
    
    public CreateProjectDto() {}

    public string Name { get; set; }
    public string Description { get; set; }
    public Guid OwnerId { get; set; }
    public List<TagDto> Tags { get; set; }
    public Guid? CompanyId { get; set; }
}