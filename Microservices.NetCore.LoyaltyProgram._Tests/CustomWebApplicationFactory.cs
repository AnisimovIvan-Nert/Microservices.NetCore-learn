using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;
using Microservices.NetCore.Shared.Store;
using Microservices.NetCore.Shared.Store.InMemory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.LoyaltyProgram.Tests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveEventFeed();
            services.AddEventFeed<InMemoryEventStore>();
            
            services.AddSingleton<IStoreSource, InMemoryStoreSource>();
        });

        builder.UseEnvironment("Development");
    }
}