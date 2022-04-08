using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.Base;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Answers.Models;
using RuOverflow.Questions.Features.Rating;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Answers;

[ExtendObjectType(typeof(Mutation))]
public class AnswerMutations
{
    private readonly IAsyncHandler<AnswerCommands.AddAnswerCommand, Answer> _addAnswerHandler;
    private readonly IDbContextFactory<RuFlowDbContext> _dbContextFactory;
    private readonly IAsyncHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity> _changeRatingHandler;

    public AnswerMutations(IAsyncHandler<AnswerCommands.AddAnswerCommand, Answer> addAnswerHandler,
        IDbContextFactory<RuFlowDbContext> dbContextFactory,
        IAsyncHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity> changeRatingHandler)
    {
        _addAnswerHandler = addAnswerHandler;
        _dbContextFactory = dbContextFactory;
        _changeRatingHandler = changeRatingHandler;
    }

    public async Task<Answer> AddAnswer(AnswerCommands.AddAnswerCommand command)
    {
        return await _addAnswerHandler.Handle(command);
    }

    public async Task<bool> LikeAnswer(Guid input)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var question = await context.Questions.FindAsync(input);

        await _changeRatingHandler.Handle(
            new RatingCommands.LikeCommand(question ?? throw new ArgumentException()));
        return true;
    }

    public async Task<bool> DislikeAnswer(Guid input)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var question = await context.Questions.FindAsync(input);

        await _changeRatingHandler.Handle(
            new RatingCommands.DislikeCommand(question ?? throw new ArgumentException()));
        return true;
    }
}
