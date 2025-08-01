using Microservices.NetCore.Shared.AspNetCore.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;
using Microservices.NetCore.ShoppingCart.Shared.ProductCatalogue;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.ProductCatalogue.Services;

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