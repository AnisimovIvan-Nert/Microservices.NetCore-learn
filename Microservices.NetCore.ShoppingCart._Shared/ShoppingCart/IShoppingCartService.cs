namespace Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;

public interface IShoppingCartService
{
    ShoppingCart Get(int id);
    Task<ShoppingCart> PostItems(int id, int[] itemIds);
    ShoppingCart DeleteItems(int id, int[] itemsIds);
}