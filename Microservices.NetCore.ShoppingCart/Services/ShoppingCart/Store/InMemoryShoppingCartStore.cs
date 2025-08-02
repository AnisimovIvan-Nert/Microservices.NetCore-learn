namespace Microservices.NetCore.ShoppingCart.Services.ShoppingCart.Store;

public class InMemoryShoppingCartStore : IShoppingCartStore
{
    private static readonly Dictionary<int, Models.ShoppingCart.ShoppingCart> _database = new();

    public ValueTask<Models.ShoppingCart.ShoppingCart> Create()
    {
        var lastId = _database.Keys.LastOrDefault();
        var id = lastId + 1;
        var shoppingCart = new Models.ShoppingCart.ShoppingCart(id);
        _database.Add(id, new Models.ShoppingCart.ShoppingCart(id));
        return ValueTask.FromResult(shoppingCart);
    }
    
    public ValueTask<Models.ShoppingCart.ShoppingCart> Get(int id)
    {
        return ValueTask.FromResult(_database[id]);
    }

    public ValueTask Save(Models.ShoppingCart.ShoppingCart shoppingCart)
    {
        return ValueTask.CompletedTask;
    }
}