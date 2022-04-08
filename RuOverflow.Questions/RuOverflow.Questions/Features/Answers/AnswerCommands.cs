namespace RuOverflow.Questions.Features.Answers;

public class AnswerCommands
{
    public record AddAnswerCommand(Guid QuestionId, string Body);
}
