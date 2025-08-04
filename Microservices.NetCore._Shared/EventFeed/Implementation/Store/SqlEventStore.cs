using System.Text.Json;
using Dapper;
using Microservices.NetCore.Shared.ConnectionSource;
using MySql.Data.MySqlClient;

namespace Microservices.NetCore.Shared.EventFeed.Implementation.Store;

public class SqlEventStore(IConnectionStringSource<IEventStore> connectionSource) : IEventStore
{
    private string? _connectionString;
    private string _streamName = "default";

    public string GetCurrentStoreStream() => _streamName;
    
    public void SetStoreStream(string streamName)
    {
        _streamName = streamName;
    }

    public async ValueTask<IEnumerable<Event>> GetEvents(long firstNumber, long lastNumber)
    {
        var @params = new
        {
            Offset = firstNumber,
            Limit = lastNumber - firstNumber + 1,
            Stream = _streamName,
        };

        const string readEventsSql =
            $"""
             SELECT
                 ROW_NUMBER() OVER (PARTITION BY {nameof(@params.Stream)} ORDER BY ID) - 1 SequenceNumber,
                 OccurredAt,
                 Name,
                 Stream,
                 Content
             FROM EventStore
             WHERE {nameof(@params.Stream)} = @{nameof(@params.Stream)}
             ORDER BY ID
             LIMIT @{nameof(@params.Limit)} OFFSET @{nameof(@params.Offset)}
             """;

        await using var connection = await CreateConnection();
        var result = await connection.QueryAsync(readEventsSql, @params);
        return result.Select(ConvertToEvent);

        Event ConvertToEvent(dynamic value)
        {
            var sequenceNumber = (long)value.SequenceNumber;
            var name = value.Name;
            var stream = value.Stream;
            var occurredAt = value.OccurredAt;
            return new Event(sequenceNumber, occurredAt, name, value.Content, stream);
        }
    }

    public async ValueTask Raise(string eventName, object content)
    {
        var jsonContent = JsonSerializer.Serialize(content);
        var @params = new
        {
            Name = eventName,
            Stream = _streamName,
            OccurredAt = DateTimeOffset.Now,
            Content = jsonContent
        };

        const string writeEventSql =
            $"""
             INSERT INTO EventStore
             (
                 {nameof(@params.Name)},
                 {nameof(@params.Stream)},
                 {nameof(@params.OccurredAt)}, 
                 {nameof(@params.Content)}
             ) VALUES 
             (
                 @{nameof(@params.Name)}, 
                 @{nameof(@params.Stream)},
                 @{nameof(@params.OccurredAt)}, 
                 @{nameof(@params.Content)}
             )
             """;
        
        await using var connection = await CreateConnection();
        await connection.ExecuteAsync(writeEventSql, @params);
    }

    private async ValueTask<MySqlConnection> CreateConnection()
    {
        _connectionString ??= await connectionSource.GetConnectionAsync();
        return new MySqlConnection(_connectionString);
    }
}