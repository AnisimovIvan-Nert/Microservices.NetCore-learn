using Microservices.NetCore.ShoppingCart.Models;
using Microservices.NetCore.ShoppingCart.Models.ProductCatalogue;

namespace Microservices.NetCore.ShoppingCart.Clients.ProductCatalogue;

public class InMemoryProductCatalogueClient : IProductCatalogueClient
{
    private static readonly Dictionary<int, ProductCatalogueItem> Catalogue;

    static InMemoryProductCatalogueClient()
    {
        const int itemsCount = 5;
        const string currency = "RUB";
        const string nameBase = "Product number ";

        Catalogue = new Dictionary<int, ProductCatalogueItem>();
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
            Catalogue.Add(i, productItem);
        }
    }

    public ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItemsBatch(int start, int size)
    {
        var products = Catalogue.Values.Skip(start).Take(size);
        return ValueTask.FromResult(products);
    }

    public ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItems(params int[] productCatalogueIds)
    {
        var products = productCatalogueIds.Select(id => Catalogue[id]);
        return ValueTask.FromResult(products);
    }
}