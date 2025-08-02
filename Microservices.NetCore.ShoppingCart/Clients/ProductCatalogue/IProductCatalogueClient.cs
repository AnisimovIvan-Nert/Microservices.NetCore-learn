using Microservices.NetCore.ShoppingCart.Models.ProductCatalogue;

namespace Microservices.NetCore.ShoppingCart.Clients.ProductCatalogue;

public interface IProductCatalogueClient
{
    ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItems(params int[] productCatalogueIds);
}