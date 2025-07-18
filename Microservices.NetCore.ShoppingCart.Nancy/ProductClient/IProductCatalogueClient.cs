using Microservices.NetCore.ShoppingCart.Nancy.ShoppingCart;

namespace Microservices.NetCore.ShoppingCart.Nancy.ProductClient;

public interface IProductCatalogueClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds);
}