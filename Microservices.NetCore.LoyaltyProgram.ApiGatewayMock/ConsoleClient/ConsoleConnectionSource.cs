using Microservices.NetCore.Shared.ConnectionSource;

namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.ConsoleClient;

public class ConsoleConnectionSource<TTarget> : IConnectionSource<TTarget, Uri>
{
    public ValueTask<Uri> GetConnectionAsync()
    {
        Console.WriteLine($"Enter {typeof(TTarget).Name} connection {nameof(Uri)}:");
        var uriString = Console.ReadLine() ?? throw new InvalidOperationException();
        return ValueTask.FromResult(new Uri(uriString));
    }
}