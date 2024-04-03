namespace SlowJsonDeserialization.Client;

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary, Dictionary<int, int> LargeField)
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}