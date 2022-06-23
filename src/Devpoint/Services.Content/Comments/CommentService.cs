using Data.Core;
using Domain.Content.Entities;
using Domain.Developers.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Content.Posts;
using Services.Developers.Developers;

namespace Services.Content.Comments;

public class CommentService : ICommentService
{
    private readonly ApplicationContext _context;
    private readonly IDeveloperService _developerService;
    private readonly IPostService _postService;

    public CommentService(ApplicationContext context, IDeveloperService developerService, IPostService postService)
    {
        _context = context;
        _developerService = developerService;
        _postService = postService;
    }

    public async Task<List<Comment>> GetAllComments()
    {
        var comments = await _context.Comments.ToListAsync();

        return comments;
    }

    public async Task<List<Comment>> GetComments(List<int> commentIds)
    {
        var comments = await _context.Comments.Where(comment => commentIds.Contains(comment.Id)).ToListAsync();

        return comments;
    }

    public async Task<Comment> GetComment(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);

        return comment;
    }

    public async Task<Developer> GetCommentAuthor(int commentId)
    {
        var comment = await GetComment(commentId);
        await _context.Entry(comment).Reference(c => c.Developer).LoadAsync();

        return comment.Developer;
    }

    public async Task<Post> GetCommentPost(int commentId)
    {
        var comment = await GetComment(commentId);
        await _context.Entry(comment).Reference(c => c.Post).LoadAsync();

        return comment.Post;
    }

    public async Task<int> CreateComment(string text, Guid authorId, int postId)
    {
        var developer = await _developerService.GetDeveloper(authorId);
        var post = await _postService.GetPost(postId);
        var comment = new Comment(text, developer, post);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return comment.Id;
    }

    public async Task UpdateText(int commentId, string text)
    {
        var comment = await GetComment(commentId);
        comment.Text = text;
        await _context.SaveChangesAsync();
    }
}