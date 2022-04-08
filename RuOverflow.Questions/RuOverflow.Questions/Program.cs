using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.Base;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Answers;
using RuOverflow.Questions.Features.Questions;
using RuOverflow.Questions.Infrastructure;
using RuOverflow.Questions.Infrastructure.ApplicationBuilderExtensions;
using RuOverflow.Questions.Infrastructure.Cache;
using RuOverflow.Questions.Infrastructure.Handlers;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;
var configuration = builder.Configuration;

services.RegisterHandlers(Assembly.GetExecutingAssembly());

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

services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<QuestionMutations>()
    .AddTypeExtension<QuestionQuery>()
    .AddTypeExtension<AnswerMutations>()
    .AddProjections();

var app = builder.Build();

app.UpdateDb<RuFlowDbContext>();

app.MapGraphQL();

app.Run();