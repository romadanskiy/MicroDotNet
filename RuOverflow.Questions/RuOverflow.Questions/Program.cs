using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.Base;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Answers;
using RuOverflow.Questions.Features.Answers.ObjectTypes;
using RuOverflow.Questions.Features.Questions;
using RuOverflow.Questions.Features.Questions.ObjectTypes;
using RuOverflow.Questions.Infrastructure;
using RuOverflow.Questions.Infrastructure.ApplicationBuilderExtensions;
using RuOverflow.Questions.Infrastructure.Cache;
using RuOverflow.Questions.Infrastructure.Handlers;
using RuOverflow.Questions.Infrastructure.Kafka;
using RuOverflow.Questions.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;
var configuration = builder.Configuration;

services.AddSingleton(configuration.GetSettings<KafkaSettings>("Kafka"));

services.AddHostedService<KafkaInitializer>();

services.AddPooledDbContextFactory<RuFlowDbContext>(
    optionsBuilder => optionsBuilder.UseNpgsql(env.IsDevelopment()
        ? builder.Configuration.GetConnectionString("local")
        : EnvironmentVariables.ConnectionString));

services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = env.IsDevelopment()
        ? configuration.GetSection("Redis:Host").Value
        : EnvironmentVariables.RedisUrl;
});

services.AddScoped<ICache, Cache>();
services.RegisterKafkaClients(configuration.GetSettings<KafkaSettings>("Kafka"));
services.RegisterProducers(Assembly.GetExecutingAssembly());
services.RegisterHandlers(Assembly.GetExecutingAssembly());

services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<QuestionMutations>()
    .AddTypeExtension<QuestionQuery>()
    .AddTypeExtension<AnswerMutations>()
    .AddType<QuestionObjectType>()
    .AddType<AnswerObjectType>()
    .AddProjections()
    .AddSorting()
    .AddFiltering();

var app = builder.Build();

app.UpdateDb<RuFlowDbContext>();

app.MapGraphQL();

app.Run();
