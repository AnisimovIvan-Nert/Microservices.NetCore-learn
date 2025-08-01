namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public interface IShoppingCartStore
{
    ValueTask<ShoppingCart> Create();
    ValueTask<ShoppingCart> Get(int id);
    ValueTask Save(ShoppingCart shoppingCart);
}