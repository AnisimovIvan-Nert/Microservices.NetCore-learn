using Microservices.NetCore.Shared.Store;
using Microservices.NetCore.Shared.Store.InMemory;

namespace Microservices.NetCore.Tests.Utilities.DockerTestContainers.InMemoryStore;

public class InMemoryStoreFixture
{
    public IStoreSource StoreSource { get; } = new InMemoryStoreSource();
}