// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.Json;
using SlowJsonDeserialization.Client;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5021/")
};
var cancellationToken = CancellationToken.None;

Console.WriteLine("Starting the application...");

foreach (var count in Enumerable.Range(1, 5))
{
    var headersReadTime = await Measure(HttpCompletionOption.ResponseHeadersRead, count);
    var contentReadTime = await Measure(HttpCompletionOption.ResponseContentRead, count);

    var diff = headersReadTime - contentReadTime;
    Console.WriteLine(
        $"Diff between {HttpCompletionOption.ResponseHeadersRead} and {HttpCompletionOption.ResponseContentRead} response is {diff.TotalMilliseconds} ms");

    Console.WriteLine();
}

return;

async Task<TimeSpan> Measure(HttpCompletionOption httpCompletionOption, int count)
{
    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"streaming?count={count}");

    var stopwatch = Stopwatch.StartNew();
    using var response = await httpClient.SendAsync(httpRequestMessage, httpCompletionOption,
        cancellationToken);

    await using var responseStream = new StreamWrapper(await response.Content.ReadAsStreamAsync(cancellationToken));

    var bufferSize = 10 * 1024 * 1024;
    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DefaultBufferSize = bufferSize
    };

    await JsonSerializer
        .DeserializeAsyncEnumerable<WeatherForecast>(responseStream, options, cancellationToken)
        .ToList();

    var elapsed = stopwatch.Elapsed;

    Console.WriteLine(
        $"Request-response {httpCompletionOption} takes {elapsed.TotalMilliseconds} ms for {responseStream.BytesRead} bytes");

    return elapsed;
}