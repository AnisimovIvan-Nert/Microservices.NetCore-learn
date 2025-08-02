namespace Microservices.NetCore.ShoppingCart.Services.ShoppingCart.Store;

public interface IShoppingCartStore
{
    ValueTask<Models.ShoppingCart.ShoppingCart> Create();
    ValueTask<Models.ShoppingCart.ShoppingCart> Get(int id);
    ValueTask Save(Models.ShoppingCart.ShoppingCart shoppingCart);
}