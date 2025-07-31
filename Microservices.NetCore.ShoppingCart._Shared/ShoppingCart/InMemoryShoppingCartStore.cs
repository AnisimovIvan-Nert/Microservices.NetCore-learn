namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public class InMemoryShoppingCartStore : IShoppingCartStore
{
    private static readonly Dictionary<int, ShoppingCart> _database = new();

    public ValueTask<ShoppingCart> Get(int userId)
    {
        if (_database.ContainsKey(userId) == false)
            _database[userId] = new ShoppingCart(userId);
        return ValueTask.FromResult(_database[userId]);
    }

    public ValueTask Save(ShoppingCart shoppingCart)
    {
        return ValueTask.CompletedTask;
    }
}