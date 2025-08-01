using Microservices.NetCore.Shared.AspNetCore.Cache;
using Microservices.NetCore.Shared.AspNetCore.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;
using Microservices.NetCore.ShoppingCart.Shared.ProductClient;
using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShoppingCartServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddEventFeed<SqlEventStore>()
            .AddCache()
            .AddProductClient()
            .AddShoppingCart();
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