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

    public Question(string title, string body, Guid authorId, List<Tag>? tags = null)
    {
        Id = Guid.NewGuid();
        Title = title;
        Body = body;
        UserId = authorId;
        Created = DateTime.UtcNow;
        Tags = tags;
    }

    private string title;

    public string Title
    {
        get => title;
        set
        {
            if (value.Length > Config.Question.MaxTitleLength)
            {
                throw new ArgumentException($"Title length must be below {Config.Question.MaxTitleLength} characters");
            }

            title = value;
            Modified = DateTime.UtcNow;
        }
    }

    private string body;

    public string Body
    {
        get => body;
        set
        {
            if (value.Length > Config.Question.MaxTitleLength)
            {
                throw new ArgumentException($"Title length must be below {Config.Question.MaxTitleLength} characters");
            }

            body = value;
            Modified = DateTime.UtcNow;
        }
    }

    public Guid UserId { get; set; }

    private List<Tag>? tags;

    public List<Tag>? Tags
    {
        get => tags;
        set
        {
            if (value?.Count > Config.Question.MaxTags)
            {
                throw new ArgumentException($"Max title count is {Config.Question.MaxTags}");
            }

            tags = value;
            Modified = DateTime.UtcNow;
        }
    }

    public List<Answer>? Answers { get; set; }
}
