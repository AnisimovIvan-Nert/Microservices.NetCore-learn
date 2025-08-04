namespace Microservices.NetCore.Shared.ConnectionSource;

public interface IConnectionStringSource<TTarget> : IConnectionSource<TTarget, string>
{
}