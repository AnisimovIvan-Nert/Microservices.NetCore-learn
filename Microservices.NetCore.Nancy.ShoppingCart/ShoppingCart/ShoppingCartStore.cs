namespace Microservices.NetCore.Nancy.ShoppingCart.ShoppingCart;

public class ShoppingCartStore : IShoppingCartStore
{
    private static readonly Dictionary<int, ShoppingCart> _database = new();

    public ShoppingCart Get(int userId)
    {
        if (_database.ContainsKey(userId) == false)
            _database[userId] = new ShoppingCart(userId);
        return _database[userId];
    }

    public void Save(ShoppingCart shoppingCart)
    {
    }
}