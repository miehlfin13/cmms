using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Synith.Caching;

public class Cache : ICache
{
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;

    public Cache(IMemoryCache cache, IConfiguration configuration)
    {
        _cache = cache;
        _configuration = configuration;
    }

    public T? Get<T>(string key)
    {
        var json = _cache.Get<string?>(key);
        if (string.IsNullOrEmpty(json)) return default;
        return JsonSerializer.Deserialize<T>(json);
    }

    /// <param name="expirationInMinutes">Uses default expiration if null or less than 1</param>
    public void Set<T>(string key, T value, int? expirationInMinutes = null)
    {
        if (expirationInMinutes == null || expirationInMinutes < 1)
        {
            expirationInMinutes = int.Parse(_configuration["Cache:DefaultExpirationInMinutes"]!);
        }

        string json = JsonSerializer.Serialize(value);

        _cache.Set(key, json, new MemoryCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromMinutes(expirationInMinutes.Value)
        });
    }
}
