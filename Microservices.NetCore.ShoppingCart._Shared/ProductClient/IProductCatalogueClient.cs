using Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;

namespace Microservices.NetCore.ShoppingCart._Shared.ProductClient;

public interface IProductCatalogueClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds);
}