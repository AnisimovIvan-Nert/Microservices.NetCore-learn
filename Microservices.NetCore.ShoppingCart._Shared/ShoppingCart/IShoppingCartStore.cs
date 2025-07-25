namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public interface IShoppingCartStore
{
    ShoppingCart Get(int userId);
    void Save(ShoppingCart shoppingCart);
}