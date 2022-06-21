using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using RatingGrpcService;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Extensions;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions.Handlers.Ask;

public record AskQuestionCommand(string Title, string Body, List<Guid>? Tags);

public class AskQuestionHandler : IAsyncHandler<AskQuestionCommand, Question>
{
    private readonly IDbContextFactory<RuFlowDbContext> _contextFactory;
    private readonly IHttpContextAccessor _accessor;

    public AskQuestionHandler([Service] IHttpContextAccessor accessor ,IDbContextFactory<RuFlowDbContext> contextFactory)
    {
        _accessor = accessor;
        _contextFactory = contextFactory;
    }

    public async Task<Question> Handle(AskQuestionCommand input)
    {
        var context = await _contextFactory.CreateDbContextAsync();
        var tags = input.Tags?.Count > 0
            ? await context.Tags.Where(x => input.Tags.Contains(x.Id)).ToListAsync()
            : null;
        
        var userId = _accessor.GetUserId();
        var question = new Question(input.Title, input.Body, userId, tags);
        
        var rating = await GetRatingByGrpcAsync(userId);

        if (rating < -50)
        {
            throw new Exception("У вас слишком низкий рейтинг, вам запрещено создавать вопросы");
        }
        
        question.Rating = 4;
        
        context.Questions.Add(question);
        await context.SaveChangesAsync();
        return question;
    }

    private static async Task<int> GetRatingByGrpcAsync(Guid userId)
    {
        using var channel = GrpcChannel.ForAddress("https://gqluserservice:8081");

        var raitingClient = new RatingGRPC.RatingGRPCClient(channel);

        var response = await raitingClient.GetRatingByUserIdAsync(new Request() { UserId = userId.ToString() });
        
        var isRaitingCorrect = int.TryParse(response.Rating, out var raiting);
        
        return isRaitingCorrect ? raiting : throw new ApplicationException();
    }
}
