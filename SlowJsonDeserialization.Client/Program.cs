// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.Json;
using SlowJsonDeserialization.Client;

var httpClient = new HttpClient()
{
    BaseAddress = new Uri("http://localhost:5021/")
};
var cancellationToken = CancellationToken.None;

await Measure(HttpCompletionOption.ResponseContentRead);
Console.WriteLine(new string('-', 64));
await Measure(HttpCompletionOption.ResponseHeadersRead);

return;

async Task Measure(HttpCompletionOption httpCompletionOption)
{
    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "streaming");
    Console.WriteLine($"Sending the request with {httpCompletionOption} option");

    var stopwatch = Stopwatch.StartNew();
    var response = await httpClient.SendAsync(httpRequestMessage, httpCompletionOption,
        cancellationToken);

    var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

    var bufferSize = 10 * 1024 * 1024;
    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DefaultBufferSize = bufferSize
    };

    var weatherForecasts = await JsonSerializer
        .DeserializeAsyncEnumerable<WeatherForecast>(responseStream, options, cancellationToken)
        .ToList();

    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

    Console.WriteLine(
        $"Request-response takes {elapsedMilliseconds} ms for {weatherForecasts.SelectMany(x => x!.LargeField).Count()} elements");
}