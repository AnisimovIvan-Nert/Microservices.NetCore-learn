using System.Text.Json;
using Dapper;
using MySql.Data.MySqlClient;

namespace Microservices.NetCore.Shared.EventFeed.Implementation.Store;

public class SqlEventStore : IEventStore
{
    //TODO link connection string
    private const string ConnectionString = "server=localhost;uid=user;pwd=password;database=EventStore";

    private string _eventType = "default";

    public void SetEventType(string eventType)
    {
        _eventType = eventType;
    }

    public async ValueTask<IEnumerable<Event>> GetEvents(long firstNumber, long lastNumber)
    {
        var @params = new
        {
            Offset = firstNumber,
            Limit = lastNumber - firstNumber,
            Type = _eventType,
        };

        const string readEventsSql =
            $"""
             SELECT
                 ROW_NUMBER() OVER (PARTITION BY {nameof(@params.Type)} ORDER BY ID) SequenceNumber,
                 OccurredAt,
                 Name,
                 Type,
                 Content
             FROM EventStore
             WHERE {nameof(@params.Type)} = @{nameof(@params.Type)}
             ORDER BY ID
             LIMIT @{nameof(@params.Limit)} OFFSET @{nameof(@params.Offset)}
             """;

        await using var connection = new MySqlConnection(ConnectionString);
        var result = await connection.QueryAsync(readEventsSql, @params);
        return result.Select(ConvertToEvent);

        Event ConvertToEvent(dynamic value)
        {
            var sequenceNumber = (long)value.SequenceNumber;
            var name = value.Name;
            var type = value.Type;
            var occurredAt = value.OccurredAt;
            return new Event(sequenceNumber, occurredAt, name, value.Content, type);
        }
    }

    public async ValueTask Raise(string eventName, object content)
    {
        var jsonContent = JsonSerializer.Serialize(content);
        var @params = new
        {
            Name = eventName,
            Type = _eventType,
            OccurredAt = DateTimeOffset.Now,
            Content = jsonContent
        };

        const string writeEventSql =
            $"""
             INSERT INTO EventStore
             (
                 {nameof(@params.Name)},
                 {nameof(@params.Type)},
                 {nameof(@params.OccurredAt)}, 
                 {nameof(@params.Content)}
             ) VALUES 
             (
                 @{nameof(@params.Name)}, 
                 @{nameof(@params.Type)},
                 @{nameof(@params.OccurredAt)}, 
                 @{nameof(@params.Content)}
             )
             """;

        await using var connection = new MySqlConnection(ConnectionString);
        await connection.ExecuteAsync(writeEventSql, @params);
    }
}