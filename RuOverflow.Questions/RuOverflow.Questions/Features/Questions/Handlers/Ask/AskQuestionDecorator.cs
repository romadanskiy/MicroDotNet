using Grpc.Net.Client;
using RatingGrpcService;
using RuOverflow.Questions.Extensions;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions.Handlers.Ask;

[Decorator]
public class AskQuestionDecorator : IAsyncHandler<AskQuestionCommand, Question>
{
    private readonly IAsyncHandler<AskQuestionCommand, Question> _handler;
    private readonly IHttpContextAccessor _accessor;

    public AskQuestionDecorator([Service] IHttpContextAccessor accessor ,IAsyncHandler<AskQuestionCommand, Question> handler)
    {
        _accessor = accessor;
        _handler = handler;
    }

    public async Task<Question> Handle(AskQuestionCommand input)
    {
        var userId = _accessor.GetUserId();
        var rating = await GetRatingByGrpcAsync(userId);
        if (rating < -50)
        {
            throw new Exception("У вас слишком низкий рейтинг, вам запрещено создавать вопросы");
        }
        return await _handler.Handle(input);
    }
    
    private static async Task<int> GetRatingByGrpcAsync(Guid userId)
    {
        using var channel = GrpcChannel.ForAddress("http://gqluserservice:50071");

        var ratingClient = new RatingGRPC.RatingGRPCClient(channel);

        try
        {
            var response = await ratingClient.GetRatingByUserIdAsync(new Request() {UserId = userId.ToString()});
            var isRatingCorrect = int.TryParse(response.Rating, out var rating);

            return isRatingCorrect ? rating : throw new ApplicationException();
        }
        catch (Exception e)
        {
            return 0;
        }
    }
}
