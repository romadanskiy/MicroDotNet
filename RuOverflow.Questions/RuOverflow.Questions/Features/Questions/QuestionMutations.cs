using HotChocolate.AspNetCore.Authorization;
using RuOverflow.Questions.Base;
using RuOverflow.Questions.Features.Questions.Handlers.Ask;
using RuOverflow.Questions.Features.Questions.Handlers.Delete;
using RuOverflow.Questions.Features.Questions.Handlers.Update;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Rating;
using RuOverflow.Questions.Infrastructure.Exceptions;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions;

[Authorize]
[ExtendObjectType(typeof(Mutation))]
public class QuestionMutations
{
    private readonly IAsyncHandler<AskQuestionCommand, Question> _askQuestionHandler;
    private readonly IAsyncHandler<ChangeRatingCommand> _changeRatingHandler;
    private readonly IAsyncHandler<UpdateQuestionCommand, Question> _updateQuestionHandler;
    private readonly IAsyncHandler<DeleteCommand> _deleteHandler;

    public QuestionMutations(IAsyncHandler<AskQuestionCommand, Question> askQuestionHandler,
        IAsyncHandler<ChangeRatingCommand> changeRatingHandler,
        IAsyncHandler<UpdateQuestionCommand, Question> updateQuestionHandler,
        IAsyncHandler<DeleteCommand> deleteHandler)
    {
        _askQuestionHandler = askQuestionHandler;
        _changeRatingHandler = changeRatingHandler;
        _updateQuestionHandler = updateQuestionHandler;
        _deleteHandler = deleteHandler;
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
    
    public async Task<bool> DeleteQuestion(Guid questionId)
    {
        await _deleteHandler.Handle(new DeleteCommand(questionId));
        return true;
    }
}
