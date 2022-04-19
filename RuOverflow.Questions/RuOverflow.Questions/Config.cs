using RuOverflow.Questions.Features.Questions.Settings;
using RuOverflow.Questions.Infrastructure.ApplicationBuilderExtensions;

namespace RuOverflow.Questions;

public static class Config
{
    private static IConfiguration _config = null!;

    public static void Initialize(IConfiguration configuration)
    {
        _config = configuration;
        Question = _config.GetSettings<QuestionSettings>("DomainSettings:Question");
    }

    public static QuestionSettings Question = null!;
}
