using RuOverflow.Questions.Features.Answers.Models;

namespace RuOverflow.Questions.Features.Answers.ObjectTypes;

public class AnswerObjectType : ObjectType<Answer>
{
    protected override void Configure(IObjectTypeDescriptor<Answer> descriptor)
    {
        descriptor.Field(x => x.Id).IsProjected();
        descriptor.Field(a => a.Rating)
            .ResolveWith<AnswerResolvers>(x => x.GetRating(default!, default!));
    }
}
