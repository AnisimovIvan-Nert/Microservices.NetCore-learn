namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public interface IShoppingCartService
{
    ValueTask<ShoppingCart> Get(int id);
    ValueTask<ShoppingCart> PostItems(int id, int[] itemIds);
    ValueTask<ShoppingCart> DeleteItems(int id, int[] itemsIds);
}