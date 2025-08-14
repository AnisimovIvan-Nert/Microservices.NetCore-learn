using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.Tests.Fakes.ConnectionSource;
using Microservices.NetCore.Tests.Utilities;
using Microservices.NetCore.Tests.Utilities.DockerTestContainers.EventStore;

namespace Microservices.NetCore.Shared.Tests.EventFeed.Store;

[Trait(Categories.TraitName, Categories.Integration.Base)]
[Trait(Categories.TraitName, Categories.Integration.Docker)]
[Trait(Categories.TraitName, Categories.Slow)]
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