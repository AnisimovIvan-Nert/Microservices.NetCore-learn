using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

namespace Microservices.NetCore.ShoppingCart.Shared.ProductClient;

public interface IProductCatalogueClient
{
    ValueTask<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(params int[] productCatalogueIds);
}