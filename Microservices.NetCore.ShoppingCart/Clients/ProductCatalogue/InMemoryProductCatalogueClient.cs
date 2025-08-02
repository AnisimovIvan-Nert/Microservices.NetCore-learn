using Microservices.NetCore.ShoppingCart.Models;
using Microservices.NetCore.ShoppingCart.Models.ProductCatalogue;

namespace Microservices.NetCore.ShoppingCart.Clients.ProductCatalogue;

public class InMemoryProductCatalogueClient : IProductCatalogueClient
{
    private static readonly Dictionary<int, ProductCatalogueItem> _catalogue;

    static InMemoryProductCatalogueClient()
    {
        const int itemsCount = 5;
        const string currency = "RUB";
        const string nameBase = "Product number ";

        _catalogue = new Dictionary<int, ProductCatalogueItem>();
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
            _catalogue.Add(i, productItem);
        }
    }

    public ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItems(params int[] productCatalogueIds)
    {
        return ValueTask.FromResult(GetItemsFromCatalogue(productCatalogueIds));
    }

    private static IEnumerable<ProductCatalogueItem> GetItemsFromCatalogue(int[] productCatalogueIds)
    {
        return GetItemsProductCatalogue(productCatalogueIds);
    }

    private static IEnumerable<ProductCatalogueItem> GetItemsProductCatalogue(int[] productCatalogueIds)
    {
        return productCatalogueIds.Select(id => _catalogue[id]);
    }
}