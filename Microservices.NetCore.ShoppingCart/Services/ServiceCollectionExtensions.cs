using Microservices.NetCore.Shared.Cache;
using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;
using Microservices.NetCore.Shared.Store.InMemory;
using Microservices.NetCore.ShoppingCart.Clients.ProductCatalogue;
using Microservices.NetCore.ShoppingCart.Services.ShoppingCart;
using Microservices.NetCore.ShoppingCart.Services.ShoppingCart.Store;

namespace Microservices.NetCore.ShoppingCart.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShoppingCartServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddEventFeed<SqlEventStore>()
            .AddCache()
            .AddProductClient()
            .AddShoppingCart()
            .AddInMemoryStoreSource();
    }
    
    private static IServiceCollection AddProductClient(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IProductCatalogueClient, ProductCatalogueClient>();
    }
    
    private static IServiceCollection AddShoppingCart(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IShoppingCartService, ShoppingCartService>()
            .AddScoped<IShoppingCartStore, ShoppingCartStore>();
    }
}