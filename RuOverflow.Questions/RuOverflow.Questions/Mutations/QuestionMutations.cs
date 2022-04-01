using RuOverflow.Questions.Commands;
using RuOverflow.Questions.Infrastructure.Handlers;
using RuOverflow.Questions.Models;

namespace RuOverflow.Questions.Mutations;

public class QuestionMutations
{
    private readonly IAsyncHandler<AskQuestionCommand, Question> _askQuestionHandler;

    public QuestionMutations(IAsyncHandler<AskQuestionCommand, Question> askQuestionHandler)
    {
        _askQuestionHandler = askQuestionHandler;
    }

    public async Task<Question> AskQuestionAsync(AskQuestionCommand input) =>
        await _askQuestionHandler.Handle(input);
}
