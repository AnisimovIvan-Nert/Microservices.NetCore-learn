using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShoppingCart(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IShoppingCartService, ShoppingCartService>()
            .AddScoped<IShoppingCartStore, ShoppingCartStore>();
    }
}