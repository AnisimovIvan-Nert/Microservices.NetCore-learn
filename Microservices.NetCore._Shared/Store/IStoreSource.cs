using Microservices.NetCore.Shared.ValueGenerators;

namespace Microservices.NetCore.Shared.Store;

public interface IStoreSource
{
    IStore<TKey, TValue> GetStore<TKey, TValue>(string identifier) where TKey : notnull;

    IReadOnlyCollection<IValueGenerator> GetOrLinkGenerators(IStore store, Func<IReadOnlyCollection<IValueGenerator>> generatorsFactory);
}