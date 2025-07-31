namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public interface IShoppingCartStore
{
    ValueTask<ShoppingCart> Get(int userId);
    ValueTask Save(ShoppingCart shoppingCart);
}