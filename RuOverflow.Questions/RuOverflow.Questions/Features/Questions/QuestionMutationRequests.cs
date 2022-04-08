namespace RuOverflow.Questions.Features.Questions;

public class QuestionCommands
{
    public record AskQuestionCommand(string Title, string Body, List<Guid>? Tags);
}
