namespace Microservices.NetCore.LoyaltyProgram.Shared.Users;

public class UsersService(IUsersStore usersStore) : IUsersService
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

    public ValueTask<int> Register(LoyaltyProgramUser user)
    {
        return usersStore.Create(user);
    }

    public async ValueTask Update(int id, LoyaltyProgramUser value)
    {
        await usersStore.Update(id, value);
    }
}