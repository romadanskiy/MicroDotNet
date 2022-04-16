using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RuOverflow.Questions.Infrastructure.Kafka;

public static class KafkaServiceExtensions
{
    public static void RegisterProducers(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan =>
            scan.FromAssemblies(new[] { assembly })
                .AddClasses(x => x
                    .AssignableTo<KafkaBaseProducer>())
                .AsSelf());
    }
}
