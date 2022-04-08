using RuOverflow.Questions.Base;
using RuOverflow.Questions.Features.Answers.Models;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Answers;

[ExtendObjectType(typeof(Mutation))]
public class AnswerMutations
{
    private readonly IAsyncHandler<AnswerCommands.AddAnswerCommand, Answer> _addAnswerHandler;

    public AnswerMutations(IAsyncHandler<AnswerCommands.AddAnswerCommand, Answer> addAnswerHandler)
    {
        _addAnswerHandler = addAnswerHandler;
    }

    public async Task<Answer> AddAnswer(AnswerCommands.AddAnswerCommand command)
    {
        return await _addAnswerHandler.Handle(command);
    }
}
