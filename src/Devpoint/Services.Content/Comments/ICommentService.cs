using Domain.Content.Entities;
using Domain.Developers.Entities;

namespace Services.Content.Comments;

public interface ICommentService
{
    public Task<List<Comment>> GetAllComments();

    public Task<List<Comment>> GetComments(List<int> commentIds);

    public Task<Comment> GetComment(int commentId);

    public Task<Developer> GetCommentAuthor(int commentId);

    public Task<Post> GetCommentPost(int commentId);

    public Task<int> CreateComment(string text, Guid authorId, int postId);

    public Task UpdateText(int commentId, string text);
}