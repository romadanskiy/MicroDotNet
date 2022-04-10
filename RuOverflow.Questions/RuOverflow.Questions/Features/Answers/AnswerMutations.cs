using RuOverflow.Questions.Base;
using RuOverflow.Questions.Features.Answers.Models;
using RuOverflow.Questions.Features.Rating;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Answers;

[ExtendObjectType(typeof(Mutation))]
public class AnswerMutations
{
    private readonly IAsyncHandler<AnswerCommands.AddAnswerCommand, Answer> _addAnswerHandler;
    private readonly IAsyncHandler<ChangeRatingCommand> _changeRatingHandler;

    public AnswerMutations(IAsyncHandler<AnswerCommands.AddAnswerCommand, Answer> addAnswerHandler,
        IAsyncHandler<ChangeRatingCommand> changeRatingHandler)
    {
        _addAnswerHandler = addAnswerHandler;
        _changeRatingHandler = changeRatingHandler;
    }

    public async Task<Answer> AddAnswer(AnswerCommands.AddAnswerCommand command)
    {
        return await _addAnswerHandler.Handle(command);
    }

    public async Task<bool> LikeAnswer(Guid answerId)
    {
        await _changeRatingHandler.Handle(new ChangeRatingCommand(answerId, EntityWithRatingType.Answer, 1));
        return true;
    }

    public async Task<bool> DislikeAnswer(Guid answerId)
    {
        await _changeRatingHandler.Handle(new ChangeRatingCommand(answerId, EntityWithRatingType.Answer, -1));
        return true;
    }
}
