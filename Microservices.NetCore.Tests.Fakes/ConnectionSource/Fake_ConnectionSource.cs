using Microservices.NetCore.Shared.ConnectionSource;

namespace Microservices.NetCore.Tests.Fakes.ConnectionSource;

public class ConnectionStringSourceFake<TTarget>(string connection) 
    : IConnectionStringSource<TTarget>
{
    public ValueTask<string> GetConnectionAsync()
    {
        return ValueTask.FromResult(connection);
    }
}