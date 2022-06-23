using Data.Core;
using Domain.Developers.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Content.Posts;
using Services.Subscriptions.Subscriptions;
using Web.API.Controllers.Content.DTOs;
using Web.API.Controllers.Developers.DTOs;
using Web.API.Controllers.Subscriptions.DTOs;

namespace Web.API.Controllers.Content;

[ApiController]
[Route("posts")]
public class PostController : Controller
{
    private readonly IPostService _postService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly ApplicationContext _context;

    public PostController(IPostService postService, ISubscriptionService subscriptionService, ApplicationContext context)
    {
        _postService = postService;
        _subscriptionService = subscriptionService;
        _context = context;
    }

    [HttpGet]
    [Route("{postId:int}")]
    public async Task<IActionResult> GetPost(int postId)
    {
        var post = await _postService.GetPost(postId);
        if (post == null)
            return NotFound();
        
        var devId = User.GetDevId();
        var subLevel = 0;
        switch (post.EntityType)
        {
            case EntityType.Developer:
                subLevel = await _subscriptionService.UserDeveloperSubscriptionLevel(devId, post.OwnerId);
                break;
            case EntityType.Project:
                subLevel = await _subscriptionService.UserProjectSubscriptionLevel(devId, post.OwnerId);
                break;
            case EntityType.Company:
                subLevel = await _subscriptionService.UserCompanySubscriptionLevel(devId, post.OwnerId);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        var result = new PostDto(post, _subscriptionService.HasSufficientSubscriptionLevel(post, devId, subLevel));
        
        return Json(result);
    }

    [HttpGet]
    [Route("{postId:int}/subscription-level")]
    public async Task<IActionResult> GetRequiredSubscriptionLevel(int postId)
    {
        var subscriptionLevel = await _postService.GetRequiredSubscriptionLevel(postId);
        var result = new SubscriptionLevelDto(subscriptionLevel);

        return Ok(result);
    }

    [HttpGet]
    [Route("{postId:int}/author")]
    public async Task<IActionResult> GetPostAuthor(int postId)
    {
        var author = await _postService.GetPostAuthor(postId);
        var result = new DeveloperDto(author);

        return Ok(result);
    }

    [HttpGet]
    [Route("{postId:int}/comments")]
    public async Task<IActionResult> GetPostComments(int postId)
    {
        var comments = await _postService.GetPostComments(postId);
        var result = comments.Select(c => new CommentDto(c));

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        var postId = await _postService.CreatePost(createPostDto.Title, createPostDto.Text, createPostDto.SubscriptionLevelId, createPostDto.DeveloperId, createPostDto.Type, createPostDto.OwnerId);

        return Ok(postId);
    }

    [HttpPut]
    [Route("{postId:int}/update")]
    public async Task<IActionResult> Update(int postId, UpdateEntityDto updateEntityDto)
    {
        var post = await _postService.GetPost(postId);
        if (post == null)
            return NotFound();
        
        var cDevId = User.GetDevId();
        if (cDevId == null)
            return Unauthorized("Unauthorized");

        if (post.DeveloperId != cDevId.Value)
            return StatusCode(403, "Forbidden! Only owner can modify posts");
        
        if (updateEntityDto.Name != null)
            post.Title = updateEntityDto.Name;

        if (updateEntityDto.ImagePath != null)
        {
            if (!string.IsNullOrWhiteSpace(post.ImagePath))
            {
                var pathToRemove = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                var fullPath = Path.Combine(pathToRemove, post.ImagePath);
                System.IO.File.Delete(fullPath);
            }

            post.ImagePath = updateEntityDto.ImagePath;
        }

        if (updateEntityDto.Description != null)
            post.Content = updateEntityDto.Description;

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    [Route("{postId:int}/update/subscription-level")]
    public async Task<IActionResult> UpdateSubscriptionLevel(int postId, int subscriptionLevelId)
    {
        await _postService.UpdateRequiredSubscriptionLevel(postId, subscriptionLevelId);

        return Ok();
    }
}