using System.Net.Http.Headers;
using System.Text.Json;

namespace Microservices.NetCore.LoyaltyProgram.Shared.EventConsumer;

public class EventSubscriberService(ILogger<EventSubscriberService> logger) : BackgroundService
{
    private const long ChunkSize = 100;
    private const int RepeatIntervalSeconds = 5;
    //TODO link uri
    private const string LoyaltyProgramBaseUri = "http://localhost:5028";
    
    private long _start;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                await ProcessEvents(cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(RepeatIntervalSeconds), cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            
            Environment.Exit(1);
        }
    }
    
    private async ValueTask ProcessEvents(CancellationToken cancellationToken)
    {
        var response = await RequestEvents(cancellationToken);
        response.EnsureSuccessStatusCode();
        var eventsData = await response.Content.ReadAsStringAsync(cancellationToken);
        
        var events = JsonSerializer.Deserialize<IEnumerable<Event>>(eventsData, JsonSerializerOptions.Web) ?? [];
        HandleEvents(events);
    }

    private async ValueTask<HttpResponseMessage> RequestEvents(CancellationToken cancellationToken)
    {
        const string jsonMediaType = "application/json";

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(LoyaltyProgramBaseUri);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(jsonMediaType));

        var start = _start;
        var end = start + ChunkSize;
        var requestUri = $"events?start={start}&end={end}";
        return await httpClient.GetAsync(requestUri, cancellationToken);
    }

    private void HandleEvents(IEnumerable<Event> events)
    {
        foreach (var @event in events)
        {
            _start = Math.Max(_start, @event.SequenceNumber + 1);
            
            Console.WriteLine(@event.SequenceNumber);
            Console.WriteLine(@event.Name);
            Console.WriteLine(@event.Content);
        }
    }
}