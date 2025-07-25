using Microservices.NetCore._Shared.EventFeed;

namespace Microservices.NetCore.LoyaltyProgram.Shared.Users;

public class UsersService(IUsersStore usersStore, IEventFeed eventFeed) : IUsersService
{
    public ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll()
    {
        return usersStore.GetAll();
    }
    
    public ValueTask<LoyaltyProgramUser> Get(int id)
    {
        return usersStore.Get(id);
    }
    
    public ValueTask<LoyaltyProgramUser?> TryGet(int id)
    {
        return usersStore.TryGet(id);
    }

    public async ValueTask<int> Register(LoyaltyProgramUser user)
    {
        var id = await usersStore.Create(user);
        _ = eventFeed.Raise("User registered", user);
        return id;
    }

    public async ValueTask Update(int id, LoyaltyProgramUser value)
    {
        await usersStore.Update(id, value);
        _ = eventFeed.Raise("User updated", value);
    }
}