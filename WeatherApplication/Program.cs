using WeatherSolution.IoC;
using WeatherSolution.IoC.Repositories;
using WheatherSolution.Application.Services;
using WheatherSolution.Domain.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWheatherApplication, WheatherApplication>();
builder.Services.AddScoped<IWheatherRepository, WheatherRepository>();
builder.Services.AddScoped<SqlContext>();
builder.Services.AddScoped<RedisConfig>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
