using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Exceptions;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions.Handlers.Delete;

public record DeleteCommand(Guid QuestionId);

public class DeleteHandler : IAsyncHandler<DeleteCommand>
{
    private readonly IDbContextFactory<RuFlowDbContext> _dbContextFactory;

    public DeleteHandler(IDbContextFactory<RuFlowDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task Handle(DeleteCommand input)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var question = await GetQuestion(input, context);
        context.Remove(question);
        await context.SaveChangesAsync();
    }

    private static async Task<Question?> GetQuestion(DeleteCommand input, RuFlowDbContext context)
    {
        var question = await context.Questions.FindAsync(input.QuestionId);

        if (question is null)
        {
            throw new EntityNotFoundException(input.QuestionId);
        }

        return question;
    }
}
