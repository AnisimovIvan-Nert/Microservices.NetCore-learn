using System.Net.Http.Json;
using System.Text.Json;
using Microservices.NetCore.LoyaltyProgram.Model;
using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;
using Microservices.NetCore.Tests.Utilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.LoyaltyProgram.Tests;

[Trait(Categories.TraitName, Categories.Integration.Base)]
[Trait(Categories.TraitName, Categories.Integration.InMemoryWebApp)]
public class ApplicationEventFeedTests : IClassFixture<CustomWebApplicationFactory>
{
    private const string BaseUserUri = "users";
    private const string BaseEventUri = "events";

    private readonly HttpClient _applicationClient;
    private readonly InMemoryEventStore _eventStore;

    public ApplicationEventFeedTests(CustomWebApplicationFactory applicationFactory)
    {
        _applicationClient = applicationFactory.CreateClient();
        var testScope = applicationFactory.Services.CreateScope();
        var eventStore = testScope.ServiceProvider.GetService<IEventStore>()
                      ?? throw new InvalidOperationException();

        if (eventStore is not InMemoryEventStore inMemoryEvent)
            throw new NotImplementedException();

        _eventStore = inMemoryEvent;

        ClearEvents();
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public async Task Events_Get__ReturnAllEventInRange(
        long actual,
        long expected,
        long rangeStart,
        long rangeEnd,
        bool sendStart,
        bool sendEnd)
    {
        for (var i = 0; i < actual; i++)
        {
            var name = i.ToString();
            var content = new { Value = i };
            await _eventStore.Raise(name, content);
        }

        var queryBuilder = new QueryBuilder();
        if (sendStart)
            queryBuilder.Add("start", rangeStart.ToString());
        if (sendEnd)
            queryBuilder.Add("end", rangeEnd.ToString());
        var uriBuilder = new UriBuilder
        {
            Path = BaseEventUri,
            Query = queryBuilder.ToString()
        };
        var uri = uriBuilder.Uri;


        var response = await _applicationClient.GetAsync(uri);


        response.EnsureSuccessStatusCode();
        var eventsData = await response.Content.ReadAsStringAsync();
        var eventsEnumerable = JsonSerializer.Deserialize<IEnumerable<Event>>(eventsData, JsonSerializerOptions.Web);
        Assert.NotNull(eventsEnumerable);
        var events = eventsEnumerable.ToArray();
        Assert.Equal(expected, events.Length);
    }

    public static TheoryData<long, long, long, long, bool, bool> GetData()
    {
        var data = new TheoryData<long, long, long, long, bool, bool>();
        var actual = 10;


        var rangeStart = long.MinValue;
        var rangeEnd = long.MaxValue;

        foreach (var sendStart in new[] { false, true })
        {
            foreach (var sendEnd in new[] { false, true })
            {
                var expected = actual;
                data.Add(actual, expected, rangeStart, rangeEnd, sendStart, sendEnd);
            }
        }


        rangeStart = 0;
        rangeEnd = 0;

        foreach (var sendStart in new[] { false, true })
        {
            foreach (var sendEnd in new[] { false, true })
            {
                var expected = sendEnd ? rangeEnd - rangeStart + 1 : actual;
                data.Add(actual, expected, rangeStart, rangeEnd, sendStart, sendEnd);
            }
        }


        rangeStart = 5;
        rangeEnd = actual;

        foreach (var sendStart in new[] { false, true })
        {
            foreach (var sendEnd in new[] { false, true })
            {
                var expected = sendStart ? rangeEnd - rangeStart : actual;
                data.Add(actual, expected, rangeStart, rangeEnd, sendStart, sendEnd);
            }
        }

        return data;
    }

    private const int RaiseEventCount = 10;

    [Fact]
    public async Task User_Post__RaiseEvent()
    {
        var user = LoyaltyProgramUserFactory.CreateDefault();
        var content = JsonContent.Create(user);


        var postResponses = new HttpResponseMessage[RaiseEventCount];
        for (var i = 0; i < RaiseEventCount; i++)
            postResponses[i] = await _applicationClient.PostAsync(BaseUserUri, content);


        var eventsEnumerable = await _eventStore.GetEvents(long.MinValue, long.MaxValue);
        var events = eventsEnumerable.ToArray();
        Assert.Equal(RaiseEventCount, events.Length);

        for (var i = 0; i < RaiseEventCount; i++)
        {
            var postResponse = postResponses[i];
            var createdUserData = await postResponse.Content.ReadAsStringAsync();
            var createdUser =
                JsonSerializer.Deserialize<LoyaltyProgramUser>(createdUserData, JsonSerializerOptions.Web);
            Assert.NotNull(createdUser);

            var creationEvent = events[i];
            var eventUserData = creationEvent.ContentJson;
            var eventUser = JsonSerializer.Deserialize<LoyaltyProgramUser>(eventUserData, JsonSerializerOptions.Web);
            Assert.Equivalent(createdUser, eventUser);
        }
    }

    [Fact]
    public async Task User_Put__RaiseEvent()
    {
        const string uriTemplate = BaseUserUri + "/{0}";

        var user = LoyaltyProgramUserFactory.CreateDefault();
        var postContent = JsonContent.Create(user);

        var createdUsers = new LoyaltyProgramUser[RaiseEventCount];
        for (var i = 0; i < RaiseEventCount; i++)
        {
            var response = await _applicationClient.PostAsync(BaseUserUri, postContent);
            var createdUserData = await response.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<LoyaltyProgramUser>(createdUserData, JsonSerializerOptions.Web)
                              ?? throw new InvalidOperationException();
            createdUsers[i] = createdUser;
        }

        ClearEvents();

        var putResponses = new HttpResponseMessage[RaiseEventCount];
        for (var i = 0; i < RaiseEventCount; i++)
        {
            var targetUser = createdUsers[i];
            var targetId = targetUser.Id;

            var newUser = LoyaltyProgramUserFactory.CreateDefault();
            newUser.Name = i.ToString();

            var uri = string.Format(uriTemplate, targetId);
            var putContent = JsonContent.Create(newUser);

            putResponses[i] = await _applicationClient.PutAsync(uri, putContent);
        }

        var eventsEnumerable = await _eventStore.GetEvents(long.MinValue, long.MaxValue);
        var events = eventsEnumerable.ToArray();
        Assert.Equal(RaiseEventCount, events.Length);

        for (var i = 0; i < RaiseEventCount; i++)
        {
            var putResponse = putResponses[i];
            var updatedUserData = await putResponse.Content.ReadAsStringAsync();
            var updatedUser =
                JsonSerializer.Deserialize<LoyaltyProgramUser>(updatedUserData, JsonSerializerOptions.Web);
            Assert.NotNull(updatedUser);

            var creationEvent = events[i];
            var eventUserData = creationEvent.ContentJson;
            var eventUser = JsonSerializer.Deserialize<LoyaltyProgramUser>(eventUserData, JsonSerializerOptions.Web);
            Assert.Equivalent(updatedUser, eventUser);
        }
    }

    private void ClearEvents()
    {
        _eventStore.Clear();
    }
}