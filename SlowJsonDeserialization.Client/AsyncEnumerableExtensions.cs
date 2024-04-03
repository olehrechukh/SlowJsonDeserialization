namespace SlowJsonDeserialization.Client;

public static class AsyncEnumerableExtensions
{
    public static async Task<List<TValue>> ToList<TValue>(this IAsyncEnumerable<TValue> enumerable)
    {
        var result = new List<TValue>();
        await foreach (var value in enumerable) result.Add(value);

        return result;
    }
}