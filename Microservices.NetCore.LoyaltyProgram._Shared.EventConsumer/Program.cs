namespace Microservices.NetCore.LoyaltyProgram.Shared.EventConsumer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<EventSubscriberService>();
        builder.Services.AddSystemd();

        var host = builder.Build();
        host.Run();
    }
}