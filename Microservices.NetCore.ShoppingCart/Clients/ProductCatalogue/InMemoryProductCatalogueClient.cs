using Microservices.NetCore.Shared.Store;
using Microservices.NetCore.ShoppingCart.Models;
using Microservices.NetCore.ShoppingCart.Models.ProductCatalogue;

namespace Microservices.NetCore.ShoppingCart.Clients.ProductCatalogue;

public class InMemoryProductCatalogueClient : IProductCatalogueClient
{
    private readonly IStore<int, ProductCatalogueItem> _store;

    public InMemoryProductCatalogueClient(IStoreSource storeSource)
    {
        _store = storeSource.GetStore<int, ProductCatalogueItem>(nameof(InMemoryProductCatalogueClient));
        FillStore();
    }

    public ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItemsBatch(int start, int size)
    {
        var products = _store.OrderBy(item => item.Id).Skip(start).Take(size);
        return ValueTask.FromResult(products);
    }

    public ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItems(params int[] productCatalogueIds)
    {
        var products = productCatalogueIds.Select(_store.Get).ToArray();
        if (products.Any(product => product is null))
            throw new InvalidOperationException();
        return ValueTask.FromResult(products.Select(product => product!));
    }

    private void FillStore()
    {
        const int itemsCount = 5;
        const string currency = "RUB";
        const string nameBase = "Product number ";
        
        for (var i = 0; i < itemsCount; i++)
        {
            var price = new Money(currency, i);
            var productItem = new ProductCatalogueItem
            {
                Id = i,
                Name = nameBase + i,
                Description = nameBase + i,
                Price = price
            };
            _store.Add(i, productItem);
        }
    }
}