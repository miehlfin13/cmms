using System.Text.Json;

namespace Synith.Core.Extensions;
public static class HttpResponseMessageExtension
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<T> DeserializeContentAsync<T>(this HttpResponseMessage response) where T : class
    {
        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), _jsonOptions)!;
    }
}
