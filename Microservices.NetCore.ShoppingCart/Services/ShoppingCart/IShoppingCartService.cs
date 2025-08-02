namespace Microservices.NetCore.ShoppingCart.Services.ShoppingCart;

public interface IShoppingCartService
{
    ValueTask<Models.ShoppingCart.ShoppingCart> Create();
    ValueTask<Models.ShoppingCart.ShoppingCart> Get(int id);
    ValueTask<Models.ShoppingCart.ShoppingCart> PostItems(int id, int[] itemIds);
    ValueTask<Models.ShoppingCart.ShoppingCart> DeleteItems(int id, int[] itemsIds);
}