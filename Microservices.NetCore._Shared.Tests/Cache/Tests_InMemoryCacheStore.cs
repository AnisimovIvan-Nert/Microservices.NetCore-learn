using System.ComponentModel;
using Microservices.NetCore.Shared.Cache;

namespace Microservices.NetCore.Shared.Tests.Cache;

public class InMemoryCacheStoreTests
{
    private const string Key = "Key";
    private const string Value = "Value";
    private const string InvalidKey = "InvalidKey";
    
    private static readonly TimeSpan NonZeroTimeToLive = TimeSpan.FromMilliseconds(1);
    private static readonly TimeSpan ZeroTimeToLive = TimeSpan.FromMilliseconds(0);
    
    [Fact]
    public void Add_WithNonZeroTimeToLive_AddValueToCache()
    {
        var store = new InMemoryCacheStore();
        
        store.Add(Key, Value, NonZeroTimeToLive);

        var storedValue = store.TryGet(Key);
        Assert.NotNull(storedValue);
        Assert.Equal(Value, storedValue);
    }
    
    [Fact]
    public void Add_WithZeroTimeToLive_AddedValueImmediatelyExpired()
    {
        var store = new InMemoryCacheStore();
        
        store.Add(Key, Value, ZeroTimeToLive);

        var storedValue = store.TryGet(Key);
        Assert.Null(storedValue);
    }
    
    [Fact]
    public void TryGet_WithValidCache_ReturnValue()
    {
        var store = new InMemoryCacheStore();
        store.Add(Key, Value, NonZeroTimeToLive);

        var storedValue = store.TryGet(Key);
        Assert.NotNull(storedValue);
        Assert.Equal(Value, storedValue);
    }
    
    [Fact]
    public void TryGet_WithInvalidKey_ReturnNull()
    {
        var store = new InMemoryCacheStore();
        store.Add(Key, Value, NonZeroTimeToLive);

        var storedValue = store.TryGet(InvalidKey);
        Assert.Null(storedValue);
    }

    [Fact]
    public async Task TryGet_WithExpiredCache_ReturnNull()
    {
        var store = new InMemoryCacheStore();
        store.Add(Key, Value, NonZeroTimeToLive);
        
        await Task.Delay(NonZeroTimeToLive);

        var storedValue = store.TryGet(Key);
        Assert.Null(storedValue);
    }
}