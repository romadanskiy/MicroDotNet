namespace RuOverflow.Questions.Features.Questions;

public record AskQuestionCommand(string Title, string Body, List<Guid>? Tags);

public record UpdateQuestionCommand(Guid Id, string Title, string Body, List<Guid>? Tags);
