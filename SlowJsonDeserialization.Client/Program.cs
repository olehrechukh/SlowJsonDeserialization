// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.Json;
using SlowJsonDeserialization.Client;

var httpClient = new HttpClient()
{
    BaseAddress = new Uri("http://localhost:5021/")
};
var cancellationToken = CancellationToken.None;

var contentReadTime = await Measure(HttpCompletionOption.ResponseContentRead);
Console.WriteLine(new string('-', 64));
var headersReadTime = await Measure(HttpCompletionOption.ResponseHeadersRead);

var diff = headersReadTime - contentReadTime;

Console.WriteLine();
Console.WriteLine(
    $"Diff between {HttpCompletionOption.ResponseHeadersRead} and {HttpCompletionOption.ResponseContentRead} response is {diff.TotalMilliseconds} ms");
return;

async Task<TimeSpan> Measure(HttpCompletionOption httpCompletionOption)
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

    var elapsed = stopwatch.Elapsed;

    Console.WriteLine(
        $"Request-response takes {elapsed.TotalMilliseconds} ms for {weatherForecasts.SelectMany(x => x!.LargeField).Count()} elements");

    return elapsed;
}