using Microsoft.EntityFrameworkCore;
using WhiteRabbit_WebApi.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var sqlConnectionString = "Host=localhost; Port=5432;Database=WhiteRabbitDb;Username=postgres;Password=access";
//var sqlConnectionString = "Host=localhost; Port=5432;Database=WhiteRabbitDb;Username=postgres;Password=1q2w3e4r5t";
builder.Services.AddDbContext<PostgreSqlContext>(options => options.UseNpgsql(sqlConnectionString));

//builder.Services.AddScoped<IDataAnimalProvider, DataAnimalProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  //  app.UseSwagger();
  //  app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
