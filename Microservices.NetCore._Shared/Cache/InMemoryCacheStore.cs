using System.Collections.Concurrent;

namespace Microservices.NetCore.Shared.Cache;

public class InMemoryCacheStore : ICacheStore
{
    private static readonly ConcurrentDictionary<string, CachedData> _cache = new();      
      
    public void Add(string key, object value, TimeSpan timeToLive)
    {
        var expiresAt = DateTimeOffset.UtcNow.Add(timeToLive);
        var cachedData = new CachedData(value, expiresAt);
        _cache[key] = cachedData;
    }

    public object? TryGet(string key)
    {
        if (_cache.TryGetValue(key, out var value) == false)
            return null;

        if (DateTimeOffset.UtcNow > value.ExpiresAt)
        {
            _cache.Remove(key, out _);
            return null;
        }

        return value.Value;
    }
    
    private record CachedData(object Value, DateTimeOffset ExpiresAt);
}