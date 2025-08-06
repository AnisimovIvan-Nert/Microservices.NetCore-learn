using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;
using Microservices.NetCore.Shared.Tests.Fakes.ConnectionSource;
using Microservices.NetCore.Tests.Utilities.DockerTestContainers.Database;
using Microservices.NetCore.Tests.Utilities.Scripts;

namespace Microservices.NetCore.Shared.Tests.EventFeed.Store;

[Trait("Category", "Integration")]
public class SqlEventStoreTests : EventStoreTestBase, IClassFixture<MySqlDatabaseFixture>
{
    private static readonly string CreateTablesScript;
    private static readonly string DropTablesScript;
    
    private readonly ConnectionStringSourceFake<IEventStore> _connectionSource;

    static SqlEventStoreTests()
    {
        var createTableScriptPath = ScriptUtilities.GetScriptPath(
            ScriptSource.EventStore, 
            ScriptTarget.MySql, 
            ScriptAction.CreateTables);
        CreateTablesScript = File.ReadAllText(createTableScriptPath);
        
        var dropTableScriptPath = ScriptUtilities.GetScriptPath(
            ScriptSource.EventStore, 
            ScriptTarget.MySql, 
            ScriptAction.DropTables);
        DropTablesScript = File.ReadAllText(dropTableScriptPath);
    }
    
    public SqlEventStoreTests(MySqlDatabaseFixture databaseFixture)
    {
        var scriptChain = DropTablesScript + '\n' + CreateTablesScript;
        databaseFixture.ExecuteAsync(scriptChain).GetAwaiter().GetResult();
        
        var connectionString = databaseFixture.GetConnectionString();
        _connectionSource = new ConnectionStringSourceFake<IEventStore>(connectionString);
    }
    
    protected override ValueTask<IEventStore> CreateEventStore()
    {
        IEventStore eventStore = new SqlEventStore(_connectionSource);
        return ValueTask.FromResult(eventStore);
    }
}