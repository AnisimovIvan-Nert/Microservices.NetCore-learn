using System.Collections;
using System.Collections.Concurrent;

namespace Microservices.NetCore.Shared.Store.InMemory;

public class InMemoryStore<TKey, TValue> : IStore<TKey, TValue>
    where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, TValue> _store = [];

    public bool Add(TKey key, TValue value)
    {
        return _store.TryAdd(key, value);
    }

    public TValue? Get(TKey key)
    {
        return _store.TryGetValue(key, out var value) 
            ? value 
            : default;
    }

    public bool Update(TKey key, TValue newValue, TValue oldValue)
    {
        return _store.TryUpdate(key, newValue, oldValue);
    }

    public bool Remove(TKey key)
    {
        return _store.Remove(key, out _);
    }

    public bool Contains(TKey key)
    {
        return _store.ContainsKey(key);
    }

    public void Clear()
    {
        _store.Clear();
    }

    public IEnumerator<TValue> GetEnumerator() => _store.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}