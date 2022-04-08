using RuOverflow.Questions.Features.Tags.Models;

namespace RuOverflow.Questions.EF;

public static class Seed
{
    public static List<Tag> Tags = new()
    {
        new("C#", "Лучший язык программирования в мире!"),
        new("Kotlin", "Лучший язык программироваия в мире!(после C#)"),
        new("Docker", "кiт"),
        new("Kafka", "Лучший брокер(после тинькоффа)")
    };
}
