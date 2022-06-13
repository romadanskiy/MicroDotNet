namespace RuOverflow.Questions.Features.Questions.Models
{
    public class ElasticQuestionSearchDto : BaseQuestionSearchDto
    {
        public List<Guid> Tags { get; set; } = new();
    }
}
