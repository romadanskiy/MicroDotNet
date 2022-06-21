using Background.Jobs.PaySubscription;
using Background.Rabbit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PaySubscriptionJob>();
builder.Services.AddHostedService<RabbitConsumer>();

var app = builder.Build();

app.Run();