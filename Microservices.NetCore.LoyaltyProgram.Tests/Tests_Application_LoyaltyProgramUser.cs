using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microservices.NetCore.LoyaltyProgram.Model;
using Microservices.NetCore.LoyaltyProgram.Services.User.Store;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.LoyaltyProgram.Tests;

public class ApplicationLoyaltyProgramUserTests : IClassFixture<CustomWebApplicationFactory>
{
    private const string BaseUri = "users";
    
    private readonly CustomWebApplicationFactory _applicationFactory;
    private readonly ILoyaltyProgramUserStore _userStore;
    
    public ApplicationLoyaltyProgramUserTests(CustomWebApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
        var testScope = _applicationFactory.Services.CreateScope();
        _userStore = testScope.ServiceProvider.GetService<ILoyaltyProgramUserStore>()
                     ?? throw new InvalidOperationException();

        if (_userStore is not LoyaltyProgramUserStore inMemoryStore)
            throw new NotImplementedException();
        
        inMemoryStore.Clear();
    }
    
    [Fact]
    public async Task Users_Get__ReturnAllUsers()
    {
        var user = CreateDefaultUser();
        await _userStore.Create(user);
        var client = _applicationFactory.CreateClient();

        var response = await client.GetAsync(BaseUri);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var userData = await response.Content.ReadAsStringAsync();
        var usersInResponse = JsonSerializer
            .Deserialize<IEnumerable<LoyaltyProgramUser>>(userData, JsonSerializerOptions.Web);
        Assert.NotNull(usersInResponse);
        AssertEqualDefaultUser(usersInResponse.Single());
    }

    [Fact]
    public async Task Users_Get_Id__ReturnUserById()
    {
        const string uriTemplate = BaseUri + "/{0}";

        var user = CreateDefaultUser();
        var id = await _userStore.Create(user);
        var uri = string.Format(uriTemplate, id);
        var client = _applicationFactory.CreateClient();

        var response = await client.GetAsync(uri);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var userData = await response.Content.ReadAsStringAsync();
        var usersInResponse = JsonSerializer.Deserialize<LoyaltyProgramUser>(userData, JsonSerializerOptions.Web);
        Assert.NotNull(usersInResponse);
        AssertEqualDefaultUser(usersInResponse);
    }
    
    
    [Fact]
    public async Task Users_Get_InvalidId__ReturnNotFound()
    {
        const string uriTemplate = BaseUri + "/{0}";

        var user = CreateDefaultUser();
        var createdId = await _userStore.Create(user);
        var invalidId = createdId + 1;
        var uri = string.Format(uriTemplate, invalidId);
        var client = _applicationFactory.CreateClient();

        var response = await client.GetAsync(uri);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Users_Post_User__AddUserToStore()
    {
        const string getUriTemplate = "/" + BaseUri + "/{0}";
        
        var user = CreateDefaultUser();
        var client = _applicationFactory.CreateClient();
        var content = JsonContent.Create(user);
        
        
        var response = await client.PostAsync(BaseUri, content);

        
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var createdUserData = await response.Content.ReadAsStringAsync();
        var createdUser = JsonSerializer.Deserialize<LoyaltyProgramUser>(createdUserData, JsonSerializerOptions.Web);
        Assert.NotNull(createdUser);
        AssertEqualDefaultUser(createdUser);
        
        var storedUsers = await _userStore.GetAll();
        var storedUser = storedUsers.Single();
        AssertEqualDefaultUser(storedUser);

        Assert.Equivalent(createdUser, storedUser, true);
        
        var location = response.Headers.Location;
        Assert.NotNull(location);
        Assert.Equal(client.BaseAddress?.Host, location.Host);
        var getUri = string.Format(getUriTemplate, createdUser.Id);
        Assert.Equal(getUri, location.AbsolutePath);
    }

    [Fact]
    public async Task Users_Put_Id_User__ReplaceUserById()
    {
        const string uriTemplate = BaseUri + "/{0}";
        const string newName = "newName";

        var user = CreateDefaultUser();
        var id = await _userStore.Create(user);
        
        var newUser = CreateDefaultUser();
        newUser.Name = newName;
        
        var uri = string.Format(uriTemplate, id);
        var content = JsonContent.Create(newUser);
        
        var client = _applicationFactory.CreateClient();

        
        var response = await client.PutAsync(uri, content);

        
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var userData = await response.Content.ReadAsStringAsync();
        var usersInResponse = JsonSerializer
            .Deserialize<LoyaltyProgramUser>(userData, JsonSerializerOptions.Web);
        Assert.NotNull(usersInResponse);
        
        newUser.Id = usersInResponse.Id;
        Assert.Equivalent(newUser, usersInResponse);
    }
    
    [Fact]
    public async Task Users_Put_InvalidId__ReturnBadRequest()
    {
        const string uriTemplate = BaseUri + "/{0}";
        const string newName = "newName";

        var user = CreateDefaultUser();
        var createdId = await _userStore.Create(user);
        var invalidId = createdId + 1;
        
        var newUser = CreateDefaultUser();
        newUser.Name = newName;
        
        var uri = string.Format(uriTemplate, invalidId);
        var content = JsonContent.Create(newUser);
        
        var client = _applicationFactory.CreateClient();

        
        var response = await client.PutAsync(uri, content);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    private const string DefaultName = "defaultName";
    
    private static LoyaltyProgramUser CreateDefaultUser()
    {
        return new LoyaltyProgramUser
        {
            Name = DefaultName,
            Settings = new LoyaltyProgramSettings
            {
                Interests = []
            }
        };
    }

    private static void AssertEqualDefaultUser(LoyaltyProgramUser user)
    {
        Assert.Equal(DefaultName, user.Name);
        Assert.Equal(0, user.LoyaltyPoints);
        Assert.Empty(user.Settings.Interests);
    }
}