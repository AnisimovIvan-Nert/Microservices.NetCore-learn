using Dapper;
using MySql.Data.MySqlClient;
using Testcontainers.MySql;

namespace Microservices.NetCore.Tests.Utilities.DockerTestContainers.Database;

internal static class MySqlDatabaseContainerUtilities
{
    private const string ImageName = "mysql";
    private const string DataBaseName = "test";
    private const string User = "user";
    private const string Password = "password";

    public static async ValueTask<MySqlContainer> RunContainer()
    {
        var mySqlContainer = new MySqlBuilder()
            .WithImage(ImageName)
            .WithDatabase(DataBaseName)
            .WithUsername(User)
            .WithPassword(Password)
            .WithAutoRemove(true)
            .Build();
        
        await mySqlContainer.StartAsync();
        return mySqlContainer;
    }

    public static async ValueTask ExecuteScript(MySqlContainer container, string sqlScript)
    {
        var connectionString = container.GetConnectionString();
        await using var connection = new MySqlConnection(connectionString);
        await connection.ExecuteAsync(sqlScript);
    }
}