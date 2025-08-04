using EventStore.Client;
using Testcontainers.EventStoreDb;

namespace Microservices.NetCore.Tests.Utilities.DockerTestContainers.EventStore;

internal static class EventStoreContainerUtilities
{
    private const string ImageName = "eventstore/eventstore";

    public static async ValueTask<EventStoreDbContainer> RunContainer()
    {
        var eventStoreContainer = new EventStoreDbBuilder()
            .WithImage(ImageName)
            .WithAutoRemove(true)
            .WithEnvironment("EVENTSTORE_MEM_DB", "True")
            .Build();
        
        await eventStoreContainer.StartAsync();
        return eventStoreContainer;
    }
}