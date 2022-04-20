using RuOverflow.Questions.Base;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Rating;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions;

[ExtendObjectType(typeof(Mutation))]
public class QuestionMutations
{
    private readonly IAsyncHandler<AskQuestionCommand, Question> _askQuestionHandler;
    private readonly IAsyncHandler<UpdateQuestionCommand, Question> _updateQuestionHandler;
    private readonly IAsyncHandler<ChangeRatingCommand> _changeRatingHandler;

    public QuestionMutations(IAsyncHandler<AskQuestionCommand, Question> askQuestionHandler,
        IAsyncHandler<ChangeRatingCommand> changeRatingHandler, IAsyncHandler<UpdateQuestionCommand, Question> updateQuestionHandler)
    {
        _askQuestionHandler = askQuestionHandler;
        _changeRatingHandler = changeRatingHandler;
        _updateQuestionHandler = updateQuestionHandler;
    }

    public async Task<Question> AskQuestionAsync(AskQuestionCommand input)
    {
        return await _askQuestionHandler.Handle(input);
    }


    public async Task<Question> UpdateQuestionAsync(UpdateQuestionCommand input)
    {
        return await _updateQuestionHandler.Handle(input);
    }

    public async Task<bool> LikeQuestion(Guid questionId)
    {
        await _changeRatingHandler.Handle(new ChangeRatingCommand(questionId, EntityWithRatingType.Question, 1));
        return true;
    }

    public async Task<bool> DislikeQuestion(Guid questionId)
    {
        await _changeRatingHandler.Handle(new ChangeRatingCommand(questionId, EntityWithRatingType.Question, -1));
        return true;
    }
}
