using Microservices.NetCore.Shared.Store;

namespace Microservices.NetCore.Shared.Cache;

public class InMemoryCacheStore : ICacheStore
{
    private readonly IStore<string, CachedData> _store;

    public InMemoryCacheStore(IStoreSource storeSource)
    {
        _store = storeSource.GetStore<string, CachedData>(nameof(InMemoryCacheStore));
    }
      
    public void Add(string key, object value, TimeSpan timeToLive)
    {
        var expiresAt = DateTimeOffset.UtcNow.Add(timeToLive);
        var cachedData = new CachedData(value, expiresAt);
        _store.Add(key, cachedData);
    }

    public object? TryGet(string key)
    {
        var cachedValue = _store.Get(key);
        if (cachedValue == null)
            return null;

        if (DateTimeOffset.UtcNow > cachedValue.ExpiresAt)
        {
            _store.Remove(key);
            return null;
        }

        return cachedValue.Value;
    }
    
    private record CachedData(object Value, DateTimeOffset ExpiresAt);
}