using Microsoft.AspNetCore.Mvc;
using Services.Content.Comments;
using Web.API.Controllers.Content.DTOs;
using Web.API.Controllers.Developers.DTOs;

namespace Web.API.Controllers.Content;

[ApiController]
[Route("comments")]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllComments()
    {
        var comments = await _commentService.GetAllComments();
        var result = comments.Select(c => new CommentDto(c));

        return Ok(result);
    }

    [HttpGet]
    [Route("{commentIds}")]
    public async Task<IActionResult> GetComments(List<int> commentIds)
    {
        var comments = await _commentService.GetComments(commentIds);
        var result = comments.Select(c => new CommentDto(c));

        return Ok(result);
    }

    [HttpGet]
    [Route("{commentId:int}")]
    public async Task<IActionResult> GetComment(int commentId)
    {
        var comment = await _commentService.GetComment(commentId);
        var result = new CommentDto(comment);

        return Ok(result);
    }

    [HttpGet]
    [Route("{commentId:int}/author")]
    public async Task<IActionResult> GetCommentAuthor(int commentId)
    {
        var author = await _commentService.GetCommentAuthor(commentId);
        var result = new DeveloperDto(author);

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateComment(string text, Guid authorId, int postId)
    {
        var commentId = await _commentService.CreateComment(text, authorId, postId);

        return Ok(commentId);
    }

    [HttpPut]
    [Route("{commentId:int}/update/text")]
    public async Task<IActionResult> UpdateText(int commentId, string text)
    {
        await _commentService.UpdateText(commentId, text);

        return Ok();
    }
}