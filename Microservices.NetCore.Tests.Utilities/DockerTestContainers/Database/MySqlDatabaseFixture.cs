using Testcontainers.MySql;

namespace Microservices.NetCore.Tests.Utilities.DockerTestContainers.Database;

public class MySqlDatabaseFixture : IDisposable
{
    private readonly MySqlContainer _mySqlContainer;
    
    public MySqlDatabaseFixture()
    {
        _mySqlContainer = MySqlDatabaseContainerUtilities.RunContainer().AsTask().Result;
    }

    public void Dispose()
    {
        _mySqlContainer.DisposeAsync();
    }
    
    public ValueTask ExecuteAsync(string sqlScript)
    {
        return MySqlDatabaseContainerUtilities.ExecuteScript(_mySqlContainer, sqlScript);
    }

    public string GetConnectionString()
    {
        return _mySqlContainer.GetConnectionString();
    }
}