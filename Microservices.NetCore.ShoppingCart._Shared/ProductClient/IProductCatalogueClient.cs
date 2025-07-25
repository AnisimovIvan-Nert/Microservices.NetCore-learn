using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

namespace Microservices.NetCore.ShoppingCart.Shared.ProductClient;

public interface IProductCatalogueClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds);
}