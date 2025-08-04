using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;

namespace Microservices.NetCore.Shared.Tests.EventFeed.Store;

public class InMemoryEventStoreTests : EventStoreTestBase
{
    public InMemoryEventStoreTests()
    {
        InMemoryEventStore.Clear();
    }

    protected override ValueTask<IEventStore> CreateEventStore()
    {
        IEventStore eventStore = new InMemoryEventStore();
        return ValueTask.FromResult(eventStore);
    }
}