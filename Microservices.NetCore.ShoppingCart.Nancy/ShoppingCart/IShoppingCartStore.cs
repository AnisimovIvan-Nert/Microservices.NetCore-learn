namespace Microservices.NetCore.ShoppingCart.Nancy.ShoppingCart;

public interface IShoppingCartStore
{
    ShoppingCart Get(int userId);
    void Save(ShoppingCart shoppingCart);
}