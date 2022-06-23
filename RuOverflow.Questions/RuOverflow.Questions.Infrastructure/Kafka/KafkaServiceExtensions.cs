using System.Net;
using System.Reflection;
using Confluent.Kafka;
using Confluent.Kafka.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RuOverflow.Questions.Infrastructure.Kafka.Base;
using RuOverflow.Questions.Infrastructure.Settings;

namespace RuOverflow.Questions.Infrastructure.Kafka;

public static class KafkaServiceExtensions
{
    public static void RegisterProducers(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan =>
            scan.FromAssemblies(new[] { assembly })
                .AddClasses(x => x
                    .AssignableTo(typeof(IProducer<>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
    }

    public static void RegisterKafkaClients(this IServiceCollection services, KafkaSettings settings)
    {
        services.AddKafkaClient(new ProducerConfig()
        {
            BootstrapServers = settings.Servers,
            Acks = Acks.Leader,
            ClientId = Dns.GetHostName(),
        });
    }
}
