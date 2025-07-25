namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public interface IShoppingCartService
{
    ShoppingCart Get(int id);
    Task<ShoppingCart> PostItems(int id, int[] itemIds);
    ShoppingCart DeleteItems(int id, int[] itemsIds);
}