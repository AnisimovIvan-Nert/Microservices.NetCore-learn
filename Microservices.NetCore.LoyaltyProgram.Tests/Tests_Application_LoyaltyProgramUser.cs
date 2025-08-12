using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microservices.NetCore.LoyaltyProgram.Model;
using Microservices.NetCore.LoyaltyProgram.Services.User.Store;
using Microservices.NetCore.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.LoyaltyProgram.Tests;

[Trait(Categories.TraitName, Categories.Integration.Base)]
[Trait(Categories.TraitName, Categories.Integration.InMemoryWebApp)]
public class ApplicationLoyaltyProgramUserTests : IClassFixture<CustomWebApplicationFactory>
{
    private const string BaseUri = "users";
    
    private readonly HttpClient _applicationClient;
    private readonly ILoyaltyProgramUserStore _userStore;
    
    public ApplicationLoyaltyProgramUserTests(CustomWebApplicationFactory applicationFactory)
    {
        _applicationClient = applicationFactory.CreateClient();
        var testScope = applicationFactory.Services.CreateScope();
        _userStore = testScope.ServiceProvider.GetService<ILoyaltyProgramUserStore>()
                     ?? throw new InvalidOperationException();

        if (_userStore is not LoyaltyProgramUserStore)
            throw new NotImplementedException();
        
        LoyaltyProgramUserStore.Clear();
    }
    
    [Fact]
    public async Task Users_Get__ReturnAllUsers()
    {
        var user = LoyaltyProgramUserFactory.CreateDefault();
        await _userStore.Create(user);

        var response = await _applicationClient.GetAsync(BaseUri);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var userData = await response.Content.ReadAsStringAsync();
        var usersInResponse = JsonSerializer
            .Deserialize<IEnumerable<LoyaltyProgramUser>>(userData, JsonSerializerOptions.Web);
        Assert.NotNull(usersInResponse);
        LoyaltyProgramUserFactory.AssertEqualDefault(usersInResponse.Single());
    }

    [Fact]
    public async Task Users_Get_Id__ReturnUserById()
    {
        const string uriTemplate = BaseUri + "/{0}";

        var user = LoyaltyProgramUserFactory.CreateDefault();
        var id = await _userStore.Create(user);
        var uri = string.Format(uriTemplate, id);

        var response = await _applicationClient.GetAsync(uri);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var userData = await response.Content.ReadAsStringAsync();
        var usersInResponse = JsonSerializer.Deserialize<LoyaltyProgramUser>(userData, JsonSerializerOptions.Web);
        Assert.NotNull(usersInResponse);
        LoyaltyProgramUserFactory.AssertEqualDefault(usersInResponse);
    }
    
    
    [Fact]
    public async Task Users_Get_InvalidId__ReturnNotFound()
    {
        const string uriTemplate = BaseUri + "/{0}";

        var user = LoyaltyProgramUserFactory.CreateDefault();
        var createdId = await _userStore.Create(user);
        var invalidId = createdId + 1;
        var uri = string.Format(uriTemplate, invalidId);

        var response = await _applicationClient.GetAsync(uri);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Users_Post_User__AddUserToStore()
    {
        const string getUriTemplate = "/" + BaseUri + "/{0}";
        
        var user = LoyaltyProgramUserFactory.CreateDefault();
        var content = JsonContent.Create(user);
        
        
        var response = await _applicationClient.PostAsync(BaseUri, content);

        
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var createdUserData = await response.Content.ReadAsStringAsync();
        var createdUser = JsonSerializer.Deserialize<LoyaltyProgramUser>(createdUserData, JsonSerializerOptions.Web);
        Assert.NotNull(createdUser);
        LoyaltyProgramUserFactory.AssertEqualDefault(createdUser);
        
        var storedUsers = await _userStore.GetAll();
        var storedUser = storedUsers.Single();
        LoyaltyProgramUserFactory.AssertEqualDefault(storedUser);

        Assert.Equivalent(createdUser, storedUser, true);
        
        var location = response.Headers.Location;
        Assert.NotNull(location);
        Assert.Equal(_applicationClient.BaseAddress?.Host, location.Host);
        var getUri = string.Format(getUriTemplate, createdUser.Id);
        Assert.Equal(getUri, location.AbsolutePath);
    }

    [Fact]
    public async Task Users_Put_Id_User__ReplaceUserById()
    {
        const string uriTemplate = BaseUri + "/{0}";
        const string newName = "newName";

        var user = LoyaltyProgramUserFactory.CreateDefault();
        var id = await _userStore.Create(user);
        
        var newUser = LoyaltyProgramUserFactory.CreateDefault();
        newUser.Name = newName;
        
        var uri = string.Format(uriTemplate, id);
        var content = JsonContent.Create(newUser);

        
        var response = await _applicationClient.PutAsync(uri, content);

        
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

        var user = LoyaltyProgramUserFactory.CreateDefault();
        var createdId = await _userStore.Create(user);
        var invalidId = createdId + 1;
        
        var newUser = LoyaltyProgramUserFactory.CreateDefault();
        newUser.Name = newName;
        
        var uri = string.Format(uriTemplate, invalidId);
        var content = JsonContent.Create(newUser);

        
        var response = await _applicationClient.PutAsync(uri, content);
        
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}