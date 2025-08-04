using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Tests.Fakes.ConnectionSource;
using Microservices.NetCore.Tests.Utilities.DockerTestContainers.EventStore;

namespace Microservices.NetCore.Shared.Tests.EventFeed.Store;

public class EventStoreTests : EventStoreTestBase, IClassFixture<EventStoreFixture>
{
    private readonly ConnectionStringSourceFake<IEventStore> _connectionSource;
    
    public EventStoreTests(EventStoreFixture eventStoreFixture)
    {
        var connectionString = eventStoreFixture.GetConnectionString();
        _connectionSource = new ConnectionStringSourceFake<IEventStore>(connectionString);
    }
    
    protected override ValueTask<IEventStore> CreateEventStore()
    {
        IEventStore eventStore = new Shared.EventFeed.Implementation.Store.EventStore(_connectionSource);
        return ValueTask.FromResult(eventStore);
    }
}