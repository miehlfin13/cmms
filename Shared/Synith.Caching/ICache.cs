namespace Synith.Caching;

public interface ICache
{
    T? Get<T>(string key);
    void Set<T>(string key, T value, int? expirationInMinutes = null);
}
