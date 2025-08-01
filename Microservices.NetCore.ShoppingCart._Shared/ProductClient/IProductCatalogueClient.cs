namespace Microservices.NetCore.ShoppingCart.Shared.ProductClient;

public interface IProductCatalogueClient
{
    ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItems(params int[] productCatalogueIds);
}