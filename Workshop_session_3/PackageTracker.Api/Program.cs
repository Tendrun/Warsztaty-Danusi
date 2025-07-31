using Microsoft.EntityFrameworkCore;
using PackageTracker.Core.Data;
using PackageTracker.Core.Repositories.EF;
using PackageTracker.Core.Repositories.Factory;
using PackageTracker.Core.TokenJWT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Configure options
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.Configure<PackageTracker.Core.Data.DataAccessSettings>(
    builder.Configuration.GetSection("DataAccess"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null)));


/// I had to change to AddScoped From AddSingleton because there was error 
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

// Register repositories using the factory
builder.Services.AddScoped(provider =>
    provider.GetRequiredService<IRepositoryFactory>().CreatePackageRepository());
builder.Services.AddScoped(provider =>
    provider.GetRequiredService<IRepositoryFactory>().CreateCarrierRepository());
builder.Services.AddScoped(provider =>
    provider.GetRequiredService<IRepositoryFactory>().CreateUserRepository());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
