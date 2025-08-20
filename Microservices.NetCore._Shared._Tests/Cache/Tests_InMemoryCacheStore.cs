using Microservices.NetCore.Shared.Cache;
using Microservices.NetCore.Shared.Store.InMemory;

namespace Microservices.NetCore.Shared.Tests.Cache;

public class InMemoryCacheStoreTests
{
    private const string Key = "Key";
    private const string Value = "Value";
    private const string InvalidKey = "InvalidKey";
    
    private static readonly TimeSpan NonZeroTimeToLive = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan ZeroTimeToLive = TimeSpan.FromMilliseconds(0);

    private readonly InMemoryCacheStore _cacheStore;
    
    public InMemoryCacheStoreTests()
    {
        var storeSource = new InMemoryStoreSource();
        _cacheStore = new InMemoryCacheStore(storeSource);
    }
    
    [Fact]
    public void Add_WithNonZeroTimeToLive_AddValueToCache()
    {
        _cacheStore.Add(Key, Value, NonZeroTimeToLive);

        var storedValue = _cacheStore.TryGet(Key);
        Assert.NotNull(storedValue);
        Assert.Equal(Value, storedValue);
    }
    
    [Fact]
    public void Add_WithZeroTimeToLive_AddedValueImmediatelyExpired()
    {
        _cacheStore.Add(Key, Value, ZeroTimeToLive);

        var storedValue = _cacheStore.TryGet(Key);
        Assert.Null(storedValue);
    }
    
    [Fact]
    public void TryGet_WithValidCache_ReturnValue()
    {
        _cacheStore.Add(Key, Value, NonZeroTimeToLive);

        var storedValue = _cacheStore.TryGet(Key);
        Assert.NotNull(storedValue);
        Assert.Equal(Value, storedValue);
    }
    
    [Fact]
    public void TryGet_WithInvalidKey_ReturnNull()
    {
        _cacheStore.Add(Key, Value, NonZeroTimeToLive);

        var storedValue = _cacheStore.TryGet(InvalidKey);
        Assert.Null(storedValue);
    }

    [Fact]
    public async Task TryGet_WithExpiredCache_ReturnNull()
    {
        _cacheStore.Add(Key, Value, NonZeroTimeToLive);
        
        await Task.Delay(NonZeroTimeToLive + TimeSpan.FromMilliseconds(1));

        var storedValue = _cacheStore.TryGet(Key);
        Assert.Null(storedValue);
    }
}