using Background.Jobs.PaySubscription;
using Background.Rabbit;

Thread.Sleep(30000);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SubscriptionsStorage>();
builder.Services.AddHostedService<PaySubscriptionJob>();
builder.Services.AddHostedService<RabbitSubscribeConsumer>();
builder.Services.AddHostedService<RabbitUnsubscribeConsumer>();
builder.Services.AddSingleton<IRabbitPublisher, RabbitPayPublisher>();

var app = builder.Build();

app.Run();

await app.Services.GetService<PaySubscriptionJob>()!.StartAsync(new CancellationToken());