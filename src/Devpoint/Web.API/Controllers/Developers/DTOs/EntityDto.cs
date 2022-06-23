using Domain.Developers.Entities;

namespace Web.API.Controllers.Developers.DTOs;

public class EntityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }

    public static EntityDto FromDeveloper(Developer developer)
    {
        return new EntityDto()
        {
            Id = developer.Id,
            Name = developer.FullName,
            ImagePath = developer.ImageFullPath,
            Description = developer.Description
        };
    }
    
    public static EntityDto FromProject(Project project)
    {
        return new EntityDto()
        {
            Id = project.Id,
            Name = project.Name,
            ImagePath = project.ImageFullPath,
            Description = project.Description
        };
    }
    
    public static EntityDto FromCompany(Company company)
    {
        return new EntityDto()
        {
            Id = company.Id,
            Name = company.Name,
            ImagePath = company.ImageFullPath,
            Description = company.Description
        };
    }
}