using Microservices.NetCore.Shared.Cache;
using Microservices.NetCore.Shared.Store.InMemory;
using Microservices.NetCore.Shared.Tests.Fakes.ConnectionSource;
using Microservices.NetCore.ShoppingCart.Clients.ProductCatalogue;
using Microservices.NetCore.Tests.Utilities;

namespace Microservices.NetCore.ShoppingCart.Tests.Clients.ProductCatalogue;

[Trait(Categories.TraitName, Categories.Integration.Base)]
[Trait(Categories.TraitName, Categories.Integration.InMemoryWebApp)]
public class ProductCatalogueClientTests : IClassFixture<ProductCatalogueApplicationFactory>
{
    private readonly IProductCatalogueClient _productCatalogueClient;
    
    public ProductCatalogueClientTests(ProductCatalogueApplicationFactory applicationFactory)
    {
        var storeSource = new InMemoryStoreSource();
        var cacheStore = new InMemoryCacheStore(storeSource);
        
        var productCatalogueAddress = applicationFactory.Server.BaseAddress.ToString();
        var connectionSource = new ConnectionStringSourceFake<IProductCatalogueClient>(productCatalogueAddress);
        
        _productCatalogueClient = new ProductCatalogueClient(cacheStore, connectionSource);
    }
}