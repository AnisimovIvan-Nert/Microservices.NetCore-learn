using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;
using Microservices.NetCore.Shared.Store;
using Microservices.NetCore.Shared.Store.InMemory;

namespace Microservices.NetCore.Shared.Tests.EventFeed.Store;

public class InMemoryEventStoreTests : EventStoreTestBase
{
    private readonly IStoreSource _storeSource = new InMemoryStoreSource();

    protected override ValueTask<IEventStore> CreateEventStore()
    {
        IEventStore eventStore = new InMemoryEventStore(_storeSource);
        return ValueTask.FromResult(eventStore);
    }
}