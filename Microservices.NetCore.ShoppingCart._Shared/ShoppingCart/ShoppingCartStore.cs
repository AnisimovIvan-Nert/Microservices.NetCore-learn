using Dapper;
using MySql.Data.MySqlClient;

namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public class ShoppingCartStore : IShoppingCartStore
{
    //TODO ling connection string
    private const string ConnectionString = "";

    public async ValueTask<ShoppingCart> Get(int userId)
    {
        var @params = new
        {
            UserId = userId
        };

        const string readItemsSql =
            $"""
             SELECT 
                 * 
             FROM ShoppingCart, ShoppingCartItems
             WHERE ShoppingCartItems.ShoppingCartId = ID
             AND ShoppingCart.UserId=@{nameof(@params.UserId)}
             """;

        await using var connection = new MySqlConnection(ConnectionString);
        var items = await connection.QueryAsync<ShoppingCartItem>(readItemsSql, @params);
        return new ShoppingCart(userId, items.ToArray());
    }

    public async ValueTask Save(ShoppingCart shoppingCart)
    {
        var deleteParams = new
        {
            shoppingCart.UserId
        };

        const string deleteAllItemsSql =
            $"""
             DELETE item 
             FROM ShoppingCartItems item INNER JOIN ShoppingCart cart
             ON item.ShoppingCartId == cart.ID AND cart.UserId==@{nameof(deleteParams.UserId)}
             """;

        const string addAllItemsSql =
            $"""
             INSERT INTO ShoppingCartItems
             (
                {nameof(ShoppingCartItem.ProductCatalogueId)},
                {nameof(ShoppingCartItem.ProductName)},
                {nameof(ShoppingCartItem.Description)},
                {nameof(ShoppingCartItem.Price.Amount)},
                {nameof(ShoppingCartItem.Price.Currency)}
             ) VALUES 
             (
                {nameof(ShoppingCartItem.ProductCatalogueId)},
                {nameof(ShoppingCartItem.ProductName)},
                {nameof(ShoppingCartItem.Description)},
                {nameof(ShoppingCartItem.Price.Amount)},
                {nameof(ShoppingCartItem.Price.Currency)}
             )
             """;

        await using var connection = new MySqlConnection(ConnectionString);
        await using var transaction = await connection.BeginTransactionAsync();
        await connection.ExecuteAsync(deleteAllItemsSql, deleteParams, transaction);
        await connection.ExecuteAsync(addAllItemsSql, shoppingCart.Items, transaction);
    }
}