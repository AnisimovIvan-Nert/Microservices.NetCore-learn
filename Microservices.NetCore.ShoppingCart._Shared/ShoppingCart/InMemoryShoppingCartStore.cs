namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public class InMemoryShoppingCartStore : IShoppingCartStore
{
    private static readonly Dictionary<int, ShoppingCart> _database = new();

    public ValueTask<ShoppingCart> Create()
    {
        var lastId = _database.Keys.LastOrDefault();
        var id = lastId + 1;
        var shoppingCart = new ShoppingCart(id);
        _database.Add(id, new ShoppingCart(id));
        return ValueTask.FromResult(shoppingCart);
    }
    
    public ValueTask<ShoppingCart> Get(int id)
    {
        return ValueTask.FromResult(_database[id]);
    }

    public ValueTask Save(ShoppingCart shoppingCart)
    {
        return ValueTask.CompletedTask;
    }
}