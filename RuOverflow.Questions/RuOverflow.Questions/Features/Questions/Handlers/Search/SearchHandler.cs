using Nest;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Questions.Requests;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions.Handlers.Search
{
    public class SearchHandler : IAsyncHandler<QuestionSearchRequest, List<QuestionSearchDto>>
    {
        private readonly IAsyncHandler<QuestionSearchRequest, List<ElasticQuestionSearchDto>> _fetchElasticHandler;
        private readonly IAsyncHandler<List<ElasticQuestionSearchDto>, List<QuestionSearchDto>> _getResponseDtoHandler;

        public SearchHandler(IAsyncHandler<QuestionSearchRequest, List<ElasticQuestionSearchDto>> fetchElasticHandler,
            IAsyncHandler<List<ElasticQuestionSearchDto>, List<QuestionSearchDto>> getResponseDtoHandler)
        {
            _fetchElasticHandler = fetchElasticHandler;
            _getResponseDtoHandler = getResponseDtoHandler;
        }

        public async Task<List<QuestionSearchDto>> Handle(QuestionSearchRequest input)
        {
            var documents = await _fetchElasticHandler.Handle(input);
            return await _getResponseDtoHandler.Handle(documents);
        }
    }
}
