using Microsoft.AspNetCore.Mvc;
using Services.Developers.Tags;
using Web.API.Controllers.Developers.DTOs;

namespace Web.API.Controllers.Developers;

[ApiController]
[Route("tags")]
public class TagController : Controller
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllTags()
    {
        var tags = await _tagService.GetAllTags();
        var result = tags.Select(t => new TagDto(t)).ToList();

        return Json(result);
    }

    [HttpGet]
    [Route("{tagIds}")]
    public async Task<IActionResult> GetTags(List<int> tagIds)
    {
        var tags = await _tagService.GetTags(tagIds);
        var result = tags.Select(t => new TagDto(t));
        
        return Ok(result);
    }

    [HttpGet]
    [Route("{tagId:int}")]
    public async Task<IActionResult> GetTag(int tagId)
    {
        var tag = await _tagService.GetTag(tagId);
        var result = new TagDto(tag);
        
        return Ok(result);
    }

    [HttpGet]
    [Route("{tagId:int}/developers")]
    public async Task<IActionResult> GetTagDevelopers(int tagId)
    {
        var developers = await _tagService.GetTagDevelopers(tagId);
        var result = developers.Select(d => new DeveloperDto(d));
        
        return Ok(result);
    }

    [HttpGet]
    [Route("{tagId:int}/projects")]
    public async Task<IActionResult> GetTagProjects(int tagId)
    {
        var projects = await _tagService.GetTagProjects(tagId);
        var result = projects.Select(p => new ProjectDto(p));

        return Ok(result);
    }

    [HttpGet]
    [Route("{tagId:int}/companies")]
    public async Task<IActionResult> GetTagCompanies(int tagId)
    {
        var companies = await _tagService.GetTagCompanies(tagId);
        var result = companies.Select(c => new CompanyDto(c));
        
        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateTag(string text)
    {
        var tagId = await _tagService.CreateTag(text);

        return Ok(tagId);
    }

    [HttpPut]
    [Route("{tagId:int}/update/text")]
    public async Task<IActionResult> UpdateText(int tagId, string text)
    {
        await _tagService.UpdateText(tagId, text);

        return Ok();
    }
}