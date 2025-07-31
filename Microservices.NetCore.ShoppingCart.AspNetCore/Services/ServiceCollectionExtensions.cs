using Microservices.NetCore.Shared.AspNetCore.EventFeed;
using Microservices.NetCore.ShoppingCart.Shared.ProductClient;
using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShoppingCartServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddEventFeed()
            .AddProductClient()
            .AddShoppingCart();
    }
    
    private static IServiceCollection AddProductClient(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IProductCatalogueClient, InMemoryProductCatalogueClient>();
    }
    
    private static IServiceCollection AddShoppingCart(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IShoppingCartService, ShoppingCartService>()
            .AddScoped<IShoppingCartStore, InMemoryShoppingCartStore>();
    }
}