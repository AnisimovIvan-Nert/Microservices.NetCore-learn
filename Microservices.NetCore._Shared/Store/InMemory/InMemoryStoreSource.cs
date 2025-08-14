using System.Collections.Concurrent;
using Microservices.NetCore.Shared.ValueGenerators;

namespace Microservices.NetCore.Shared.Store.InMemory;

public class InMemoryStoreSource : IStoreSource
{
    private readonly ConcurrentDictionary<string, IStore> _storeCollection = new();
    private readonly ConcurrentDictionary<IStore, IReadOnlyCollection<IValueGenerator>> _storeToGeneratorsLinks = new();
    
    public IStore<TKey, TValue> GetStore<TKey, TValue>(string identifier) where TKey : notnull
    {
        var store = _storeCollection.GetOrAdd(identifier, new InMemoryStore<TKey, TValue>());

        if (store is not IStore<TKey, TValue> typedStore)
            throw new InvalidOperationException();

        return typedStore;
    }

    public IReadOnlyCollection<IValueGenerator> GetOrLinkGenerators(IStore store, Func<IReadOnlyCollection<IValueGenerator>> generatorsFactory)
    {
        return _storeToGeneratorsLinks.GetOrAdd(store, _ => generatorsFactory());
    }
}