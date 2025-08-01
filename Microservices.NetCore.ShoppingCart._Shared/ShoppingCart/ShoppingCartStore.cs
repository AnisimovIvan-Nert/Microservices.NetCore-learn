using Dapper;
using MySql.Data.MySqlClient;

namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public class ShoppingCartStore : IShoppingCartStore
{
    //TODO ling connection string
    private const string ConnectionString = "server=localhost;uid=user;pwd=password;database=ShoppingCart";

    public async ValueTask<ShoppingCart> Create()
    {
        const string createShoppingCartSql =
            """
            INSERT INTO ShoppingCart VALUES ();
            SELECT LAST_INSERT_ID();
            """;

        await using var connection = new MySqlConnection(ConnectionString);
        var id = await connection.QuerySingleAsync<int>(createShoppingCartSql);
        return new ShoppingCart(id);
    }

    public async ValueTask<ShoppingCart> Get(int id)
    {
        var @params = new
        {
            ShoppingCartId = id
        };

        const string readItemsSql =
            $"""
             SELECT 
                 * 
             FROM ShoppingCartItems
             WHERE ShoppingCartId=@{nameof(@params.ShoppingCartId)}
             """;

        await using var connection = new MySqlConnection(ConnectionString);
        var items = await connection.QueryAsync<ShoppingCartItem>(readItemsSql, @params);
        return new ShoppingCart(id, items.ToArray());
    }

    public async ValueTask Save(ShoppingCart shoppingCart)
    {
        var deleteParams = new
        {
            ShoppingCartId = shoppingCart.Id
        };

        const string deleteAllItemsSql =
            $"""
             DELETE item 
             FROM ShoppingCartItems item
             WHERE item.ShoppingCartId=@{nameof(deleteParams.ShoppingCartId)}
             """;

        object ConvertToAddModel(ShoppingCartItem item) => new
        {
            item.ShoppingCartId,
            item.ProductId,
            item.Name,
            item.Description,
            item.Price.Amount,
            item.Price.Currency
        };

        const string addAllItemsSql =
            $"""
             INSERT INTO ShoppingCartItems
             (
                {nameof(ShoppingCartItem.ShoppingCartId)},
                {nameof(ShoppingCartItem.ProductId)},
                {nameof(ShoppingCartItem.Name)},
                {nameof(ShoppingCartItem.Description)},
                {nameof(ShoppingCartItem.Price.Amount)},
                {nameof(ShoppingCartItem.Price.Currency)}
             ) VALUES 
             (
                @{nameof(ShoppingCartItem.ShoppingCartId)},
                @{nameof(ShoppingCartItem.ProductId)},
                @{nameof(ShoppingCartItem.Name)},
                @{nameof(ShoppingCartItem.Description)},
                @{nameof(ShoppingCartItem.Price.Amount)},
                @{nameof(ShoppingCartItem.Price.Currency)}
             )
             """;

        await using var connection = new MySqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        await connection.ExecuteAsync(deleteAllItemsSql, deleteParams, transaction);
        var addModels = shoppingCart.Items.Select(ConvertToAddModel);
        await connection.ExecuteAsync(addAllItemsSql, addModels, transaction);
        await transaction.CommitAsync();
    }
}