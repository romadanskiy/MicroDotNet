using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Infrastructure;
using RuOverflow.Questions.Infrastructure.ApplicationBuilderExtensions;
using RuOverflow.Questions.Infrastructure.Handlers;
using RuOverflow.Questions.Mutations;
using RuOverflow.Questions.Queries;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

services.RegisterHandlers(Assembly.GetExecutingAssembly());

services.AddPooledDbContextFactory<RuFlowDbContext>(
    optionsBuilder => optionsBuilder.UseSqlServer(env.IsDevelopment()
        ? builder.Configuration.GetConnectionString("local")
        : EnvironmentVariables.ConnectionString));

services.AddGraphQLServer()
    .AddQueryType<QuestionQuery>()
    .AddMutationType<QuestionMutations>()
    .AddProjections();

var app = builder.Build();

app.UpdateDb<RuFlowDbContext>();

app.MapGraphQL();

app.Run();
