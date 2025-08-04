using Testcontainers.EventStoreDb;

namespace Microservices.NetCore.Tests.Utilities.DockerTestContainers.EventStore;

public class EventStoreFixture : IDisposable
{
    private readonly EventStoreDbContainer _eventStoreContainer;
    
    public EventStoreFixture()
    {
        _eventStoreContainer = EventStoreContainerUtilities.RunContainer().AsTask().Result;
    }

    public void Dispose()
    {
        _eventStoreContainer.DisposeAsync();
    }
    
    public string GetConnectionString()
    {
        return _eventStoreContainer.GetConnectionString();
    }
}