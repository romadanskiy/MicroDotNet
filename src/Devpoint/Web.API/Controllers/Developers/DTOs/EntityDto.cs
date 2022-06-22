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
            Name = developer.Name,
            ImagePath = developer.ImagePath,
            Description = developer.Description
        };
    }
    
    public static EntityDto FromProject(Project project)
    {
        return new EntityDto()
        {
            Id = project.Id,
            Name = project.Name,
            ImagePath = project.ImagePath,
            Description = project.Description
        };
    }
    
    public static EntityDto FromCompany(Company company)
    {
        return new EntityDto()
        {
            Id = company.Id,
            Name = company.Name,
            ImagePath = company.ImagePath,
            Description = company.Description
        };
    }
}