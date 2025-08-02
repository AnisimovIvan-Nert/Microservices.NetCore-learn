using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;
using Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue;
using Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue.Store;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductCatalogueServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddEventFeed<SqlEventStore>()
            .AddProductCatalogue();
    }
    
    private static IServiceCollection AddProductCatalogue(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IProductCatalogueService, ProductCatalogueService>()
            .AddScoped<IProductStore, ProductStore>();
    }
}