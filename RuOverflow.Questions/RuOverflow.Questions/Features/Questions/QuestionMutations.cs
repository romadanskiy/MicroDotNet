using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.Base;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Rating;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions;

[ExtendObjectType(typeof(Mutation))]
public class QuestionMutations
{
    private readonly IAsyncHandler<QuestionCommands.AskQuestionCommand, Question> _askQuestionHandler;
    private readonly IAsyncHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity> _changeRatingHandler;
    private readonly IDbContextFactory<RuFlowDbContext> _dbContextFactory;

    public QuestionMutations(IAsyncHandler<QuestionCommands.AskQuestionCommand, Question> askQuestionHandler,
        IDbContextFactory<RuFlowDbContext> dbContextFactory,
        IAsyncHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity> changeRatingHandler)
    {
        _askQuestionHandler = askQuestionHandler;
        _dbContextFactory = dbContextFactory;
        _changeRatingHandler = changeRatingHandler;
    }

    public async Task<Question> AskQuestionAsync(QuestionCommands.AskQuestionCommand input) =>
        await _askQuestionHandler.Handle(input);

    public async Task<bool> LikeQuestion(Guid input)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var question = await context.Questions.FindAsync(input);

        await _changeRatingHandler.Handle(
            new RatingCommands.LikeCommand(question ?? throw new ArgumentException()));
        return true;
    }

    public async Task<bool> DislikeQuestion(Guid input)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var question = await context.Questions.FindAsync(input);

        await _changeRatingHandler.Handle(
            new RatingCommands.DislikeCommand(question ?? throw new ArgumentException()));
        return true;
    }
}
