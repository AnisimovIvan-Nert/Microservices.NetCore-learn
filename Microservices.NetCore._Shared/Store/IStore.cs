namespace Microservices.NetCore.Shared.Store;

public interface IStore
{
    void Clear();
}

public interface IStore<in TKey, TValue> : IStore, IEnumerable<TValue>
    where TKey : notnull
{
    bool Add(TKey key, TValue value);
    TValue? Get(TKey key);
    bool Update(TKey key, TValue newValue, TValue oldValue);
    bool Remove(TKey key);
    
    bool Contains(TKey key);
}