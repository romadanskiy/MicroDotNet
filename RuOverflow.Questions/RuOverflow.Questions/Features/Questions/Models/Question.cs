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

    public Question(string title, string body, List<Tag>? tags = null)
    {
        Id = Guid.NewGuid();
        Title = title;
        Body = body;
        UserId = Guid.NewGuid(); // временное решение
        Created = DateTime.UtcNow;
        Tags = tags;
    }

    public string Title { get; set; }
    public string Body { get; set; }

    public Guid UserId { get; set; }

    public List<Tag>? Tags { get; set; }

    public List<Answer>? Answers { get; set; }
}
