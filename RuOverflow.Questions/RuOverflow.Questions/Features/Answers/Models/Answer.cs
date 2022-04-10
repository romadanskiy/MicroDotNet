using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Rating;

namespace RuOverflow.Questions.Features.Answers.Models;

public class Answer : HasRatingEntity
{
#nullable disable
    protected Answer()
    {
    }
#nullable enable

    public Answer(Guid questionId, string body, Guid userId)
    {
        QuestionId = questionId;
        Body = body;
        UserId = userId;
        Created = DateTime.UtcNow;
    }

    public Answer(Question question, string body, Guid userId) : this(question.Id, body, userId)
    {
    }

    public string Body { get; set; }

    public Guid UserId { get; set; }

    public Guid QuestionId { get; set; }

    public Question? Question { get; set; }
}
