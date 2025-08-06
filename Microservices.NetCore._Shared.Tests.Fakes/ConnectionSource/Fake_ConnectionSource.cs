using Microservices.NetCore.Shared.ConnectionSource;

namespace Microservices.NetCore.Shared.Tests.Fakes.ConnectionSource;

public class ConnectionStringSourceFake<TTarget>(string connection) 
    : IConnectionStringSource<TTarget>
{
    public ValueTask<string> GetConnectionAsync()
    {
        return ValueTask.FromResult(connection);
    }
}