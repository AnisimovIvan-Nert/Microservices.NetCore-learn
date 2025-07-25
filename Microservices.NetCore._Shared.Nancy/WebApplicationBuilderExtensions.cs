using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.Shared.Nancy;

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