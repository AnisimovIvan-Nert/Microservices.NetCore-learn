using Microservices.NetCore.ShoppingCart._Shared.EventFeed;
using Microservices.NetCore.ShoppingCart._Shared.ProductClient;
using Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShoppingCartServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IShoppingCartStore, ShoppingCartStore>()
            .AddScoped<IProductCatalogueClient, InMemoryProductCatalogueClient>()
            .AddScoped<IEventStore, EventStore>();
    }
}