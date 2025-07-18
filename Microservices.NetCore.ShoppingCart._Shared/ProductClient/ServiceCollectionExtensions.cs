using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.ShoppingCart._Shared.ProductClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductClient(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IProductCatalogueClient, InMemoryProductCatalogueClient>();
    }
}