using Background.Jobs.PaySubscription;
using Background.Rabbit;

Thread.Sleep(30000);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PaySubscriptionJob>();
builder.Services.AddHostedService<RabbitSubscribeConsumer>();
builder.Services.AddHostedService<RabbitUnsubscribeConsumer>();
builder.Services.AddSingleton<IRabbitPublisher, RabbitPayPublisher>();

var app = builder.Build();

app.Run();