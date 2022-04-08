using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.Commands;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Infrastructure.Handlers;
using RuOverflow.Questions.Models;

namespace RuOverflow.Questions.Handlers;

public class AskQuestionHandler : IAsyncHandler<AskQuestionCommand, Question>
{
    private readonly IDbContextFactory<RuFlowDbContext> _contextFactory;

    public AskQuestionHandler(IDbContextFactory<RuFlowDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Question> Handle(AskQuestionCommand input)
    {
        var context = await _contextFactory.CreateDbContextAsync();
        var tags = input.Tags?.Count > 0
            ? await context.Tags.Where(x => input.Tags.Contains(x.Id)).ToListAsync()
            : null;

        //ToDo take this from jwt when auth is ready
        var userId = Guid.NewGuid();

        var question = new Question(input.Title, input.Body, userId, tags);
        context.Questions.Add(question);
        await context.SaveChangesAsync();
        return question;
    }
}
