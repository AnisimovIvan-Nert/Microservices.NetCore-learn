using Microservices.NetCore.Shared.Nancy;
using Nancy.Owin;

namespace Microservices.NetCore.LoyaltyProgram.Nancy;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AllowSynchronousIO();

        var app = builder.Build();
        app.UseOwin().UseNancy();
        app.Run();
    }
}