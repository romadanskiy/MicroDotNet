using RuOverflow.Questions.Features.Answers.Models;
using RuOverflow.Questions.Features.Rating;
using RuOverflow.Questions.Features.Tags.Models;

namespace RuOverflow.Questions.Features.Questions.Models;

public class Question : HasRatingEntity
{
#nullable disable
    protected Question()
    {
    }
#nullable enable

    public Question(string title, string body, Guid userId, List<Tag>? tags = null)
    {
        Id = Guid.NewGuid();
        Title = title;
        Body = body;
        UserId = userId;
        Created = DateTime.UtcNow;
        Tags = tags;
    }

    public string Title { get; protected set; }
    public string Body { get; protected set; }

    public Guid UserId { get; protected set; }

    public List<Tag>? Tags { get; protected set; }

    public List<Answer>? Answers { get; protected set; }
}
