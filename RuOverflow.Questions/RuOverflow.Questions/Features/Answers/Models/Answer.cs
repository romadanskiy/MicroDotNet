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

    public string Body { get; protected set; }

    public Guid UserId { get; protected set; }

    public Guid QuestionId { get; protected set; }

    public Question? Question { get; protected set; }
}
