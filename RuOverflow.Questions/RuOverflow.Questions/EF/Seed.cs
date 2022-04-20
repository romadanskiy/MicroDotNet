using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Tags.Models;

namespace RuOverflow.Questions.EF;

public static class Seed
{
    public static List<Tag> Tags = new()
    {
        new("C#", "Лучший язык программирования в мире!"),
        new("Kotlin", "Лучший язык программироваия в мире!(после C#)"),
        new("Docker", "кiт"),
        new("Kafka", "Лучший брокер(после тинькоффа)"),
    };


    public static List<Question> Questions = new()
    {
        new("Контент1", "Тело1?", Guid.NewGuid()),
        new("Контент2", "Тело2?", Guid.NewGuid()),
        new("Контент3", "Тело3?", Guid.NewGuid()),
        new("Контент4", "Тело4?", Guid.NewGuid()),
        new("Контент5", "Тело5?", Guid.NewGuid()),
    };
}
