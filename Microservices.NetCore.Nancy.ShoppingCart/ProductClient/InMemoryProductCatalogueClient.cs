using Microservices.NetCore.Nancy.ShoppingCart.ShoppingCart;

namespace Microservices.NetCore.Nancy.ShoppingCart.ProductClient;

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
                ProductId = i.ToString(),
                ProductName = nameBase + i,
                ProductDescription = nameBase + i,
                Price = price
            };
            _catalogue.Add(i, productItem);
        }
    }

    public Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds)
    {
        return Task.FromResult(GetItemsFromCatalogue(productCatalogueIds));
    }

    private static IEnumerable<ShoppingCartItem> GetItemsFromCatalogue(int[] productCatalogueIds)
    {
        var productItems = GetItemsProductCatalogue(productCatalogueIds);
        return ConvertToShoppingCartItems(productItems);
    }

    private static IEnumerable<ProductCatalogueItem> GetItemsProductCatalogue(int[] productCatalogueIds)
    {
        return productCatalogueIds.Select(id => _catalogue[id]);
    }

    private static IEnumerable<ShoppingCartItem> ConvertToShoppingCartItems(
        IEnumerable<ProductCatalogueItem> productItems)
    {
        return productItems.Select(ConvertToShoppingCartItem);
    }

    private static ShoppingCartItem ConvertToShoppingCartItem(ProductCatalogueItem item)
    {
        var id = int.Parse(item.ProductId);
        return new ShoppingCartItem(id, item.ProductName, item.ProductDescription, item.Price);
    }
}