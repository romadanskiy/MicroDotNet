using RuOverflow.Questions.Features.Tags.Models;

namespace RuOverflow.Questions.Features.Questions.Models
{
    public class QuestionSearchDto: BaseQuestionSearchDto
    {
        public List<Tag> Tags { get; set; } = new();
    }
}
