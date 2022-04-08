var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHos

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
