using Microservices.NetCore.LoyaltyProgram.Model;
using Microservices.NetCore.LoyaltyProgram.Services.User.Store;

namespace Microservices.NetCore.LoyaltyProgram.Tests.Services.User.Store;

public class LoyaltyProgramUserStoreTests
{
    private readonly LoyaltyProgramUserStore _store;
    
    public LoyaltyProgramUserStoreTests()
    {
        _store = new LoyaltyProgramUserStore();
        _store.Clear();
    }
    
    [Fact]
    public async Task Create__GenerateId_And_AddUserToStore()
    {
        var user = CreateDefaultUser();
        var defaultId = user.Id;

        var createdUserId = await _store.Create(user);
        
        var storedUser = await _store.Get(createdUserId);
        AssertEqualDefaultUser(storedUser);
        Assert.NotEqual(defaultId, storedUser.Id);
    }

    [Fact]
    public async Task Get_ReturnUserById()
    {
        var user = CreateDefaultUser();
        var createdUserId = await _store.Create(user);
        
        var storedUser = await _store.Get(createdUserId);
        
        AssertEqualDefaultUser(storedUser);
    }
    
    [Fact]
    public async Task Get_WithInvalidId_ThrowInvalidOperationException()
    {
        var user = CreateDefaultUser();
        var createdUserId = await _store.Create(user);
        var id = createdUserId + 1;

        await Assert.ThrowsAsync<InvalidOperationException>(() => _store.Get(id).AsTask());
    }

    [Fact]
    public async Task TryGet_ReturnUserById()
    {
        var user = CreateDefaultUser();
        var createdUserId = await _store.Create(user);
        
        var storedUser = await _store.TryGet(createdUserId);

        Assert.NotNull(storedUser);
        AssertEqualDefaultUser(storedUser);
    }
    
    [Fact]
    public async Task TryGet_WithInvalidId_ReturnNull()
    {
        var user = CreateDefaultUser();
        var createdUserId = await _store.Create(user);
        var id = createdUserId + 1;
        
        var storedUser = await _store.TryGet(id);

        Assert.Null(storedUser);
    }
    
    [Fact]
    public async Task Update_ReplaceUserWithGivenIdInStore()
    {
        const string newName = "newName";
        
        var user = CreateDefaultUser();
        var createdUserId = await _store.Create(user);

        var newUser = CreateDefaultUser();
        newUser.Name = newName;
        var result = await _store.Update(createdUserId, newUser);

        var storedUser = await _store.Get(createdUserId);
        Assert.True(result);
        Assert.Equal(newName, storedUser.Name);
    }
    
    [Fact]
    public async Task Update_WithInvalidId_ReturnFalse()
    {
        var user = CreateDefaultUser();
        var createdUserId = await _store.Create(user);
        var id = createdUserId + 1;

        var result = await _store.Update(id, user);
        
        Assert.False(result);
    }

    private const int CreationCount = 10;

    [Fact]
    public async Task Creat_MultipleTime_GenerateUniqueId()
    {
        var createdUserIds = new int[CreationCount];
        for (var i = 0; i < CreationCount; i++)
        {
            var user = CreateDefaultUser();
            createdUserIds[i] = await _store.Create(user);
        }
        
        var idsHashSet = new HashSet<int>(createdUserIds);
        Assert.Equal(createdUserIds.Length, idsHashSet.Count);
    }
    
    [Fact]
    public async Task GetAll_ReturnAllCreatedUsers()
    {
        var createdUserIds = new int[CreationCount];
        for (var i = 0; i < CreationCount; i++)
        {
            var user = CreateDefaultUser();
            createdUserIds[i] = await _store.Create(user);
        }

        var storedUsers = await _store.GetAll();
        var storedUsersIds = storedUsers.Select(storedUser => storedUser.Id).ToArray();
        Assert.Equal(createdUserIds.Length, storedUsersIds.Length);
        foreach (var createdUserId in createdUserIds)
            Assert.Contains(createdUserId, storedUsersIds);
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