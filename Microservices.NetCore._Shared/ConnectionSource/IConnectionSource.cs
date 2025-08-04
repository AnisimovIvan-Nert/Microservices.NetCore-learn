namespace Microservices.NetCore.Shared.ConnectionSource;

public interface IConnectionSource<TTarget, TConnection>
{
    ValueTask<TConnection> GetConnectionAsync();
}