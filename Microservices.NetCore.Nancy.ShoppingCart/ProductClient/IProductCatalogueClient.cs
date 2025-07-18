using Microservices.NetCore.Nancy.ShoppingCart.ShoppingCart;

namespace Microservices.NetCore.Nancy.ShoppingCart.ProductClient;

public interface IProductCatalogueClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds);
}