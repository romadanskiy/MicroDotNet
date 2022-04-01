namespace RuOverflow.Questions.Commands;

public class AskQuestionCommand
{
    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public List<Guid>? Tags { get; set; }
}
