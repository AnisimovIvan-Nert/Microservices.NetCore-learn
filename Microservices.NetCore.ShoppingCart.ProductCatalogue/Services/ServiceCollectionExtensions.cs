using Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue;
using Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue.Store;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductCatalogueServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IProductCatalogueService, ProductCatalogueService>()
            .AddScoped<IProductStore, ProductStore>();
    }
}