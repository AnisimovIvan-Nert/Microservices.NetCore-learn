using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Microservices.NetCore.Nancy.HelloWorld;

public static class WebApplicationBuilderExtensions
{
    public static void AllowSynchronousIO(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
    }
}