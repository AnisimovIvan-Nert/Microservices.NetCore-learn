namespace Microservices.NetCore.Nancy.ShoppingCart.ShoppingCart;

public interface IShoppingCartStore
{
    ShoppingCart Get(int userId);
    void Save(ShoppingCart shoppingCart);
}