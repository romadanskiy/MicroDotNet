using RuOverflow.Questions.Features.Questions.Models;

namespace RuOverflow.Questions.Features.Questions.ObjectTypes;

public class QuestionObjectType : ObjectType<Question>
{
    protected override void Configure(IObjectTypeDescriptor<Question> descriptor)
    {
        descriptor.Field(x => x.Answers).Ignore();

        descriptor.Field(x => x.Id).IsProjected();

        descriptor.Field(q => q.Rating)
            .ResolveWith<QuestionResolvers>(x => x.GetRating(default!, default!));

        descriptor
            .Field("AnswerList")
            .ResolveWith<QuestionResolvers>(x => x.GetAnswers(default!, default!));
    }
}
