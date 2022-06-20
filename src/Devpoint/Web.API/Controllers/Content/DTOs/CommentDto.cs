using Domain.Content.Entities;

namespace Web.API.Controllers.Content.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public string Text { get; set; }

    public CommentDto(Comment comment)
    {
        Id = comment.Id;
        Text = comment.Text;
    }
}