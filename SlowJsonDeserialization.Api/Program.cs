using Microsoft.AspNetCore.Mvc;
using SlowJsonDeserialization.Client;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/streaming", ([FromQuery] int count) => EnumerateWeatherForecasts(count));

app.Run();
return;

async IAsyncEnumerable<WeatherForecast> EnumerateWeatherForecasts(int count)
{
    foreach (var index in Enumerable.Range(1, count))
    {
        yield return new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)],
            Enumerable.Range(0, 1_000_000).ToDictionary(x => x, x => x)
        );

        await Task.Delay(10);
    }
}