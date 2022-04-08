using RuOverflow.Questions.Base;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions;

[ExtendObjectType(typeof(Mutation))]
public class QuestionMutations
{
    private readonly IAsyncHandler<QuestionCommands.AskQuestionCommand, Question> _askQuestionHandler;

    public QuestionMutations(IAsyncHandler<QuestionCommands.AskQuestionCommand, Question> askQuestionHandler)
    {
        _askQuestionHandler = askQuestionHandler;
    }

    public async Task<Question> AskQuestionAsync(QuestionCommands.AskQuestionCommand input) =>
        await _askQuestionHandler.Handle(input);
}
