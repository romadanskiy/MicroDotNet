using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions.Handlers;

public class UpdateQuestionHandler : IAsyncHandler<UpdateQuestionCommand, Question>
{
    private readonly IDbContextFactory<RuFlowDbContext> _contextFactory;

    public UpdateQuestionHandler(IDbContextFactory<RuFlowDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Question> Handle(UpdateQuestionCommand input)
    {
        var context = await _contextFactory.CreateDbContextAsync();
        var question = await context.Questions
            .Include(x=>x.Tags)
            .FirstOrDefaultAsync(x => x.Id == input.Id);

        if (question is null)
        {
            throw new ArgumentException($"Could not find question with id = {input.Id}");
        }

        question.Title = input.Title;
        question.Body = input.Body;
        question.Tags = input.Tags is not null && input.Tags.Count > 0
            ? await context.Tags.Where(x => input.Tags.Contains(x.Id)).ToListAsync()
            : null;
        await context.SaveChangesAsync();
        return question;
    }
}
