using Microservices.NetCore.LoyaltyProgram.Model;
using Microservices.NetCore.LoyaltyProgram.Services.User.Store;
using Microservices.NetCore.Shared.EventFeed;

namespace Microservices.NetCore.LoyaltyProgram.Services.User;

public class LoyaltyProgramUserService(
    ILoyaltyProgramUserStore loyaltyProgramUserStore, 
    IEventFeed eventFeed) 
    : ILoyaltyProgramUserService
{
    public ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll()
    {
        return loyaltyProgramUserStore.GetAll();
    }
    
    public ValueTask<LoyaltyProgramUser> Get(int id)
    {
        return loyaltyProgramUserStore.Get(id);
    }
    
    public ValueTask<LoyaltyProgramUser?> TryGet(int id)
    {
        return loyaltyProgramUserStore.TryGet(id);
    }

    public async ValueTask<int> Register(LoyaltyProgramUser user)
    {
        var id = await loyaltyProgramUserStore.Create(user);
        _ = eventFeed.Raise("User registered", user);
        return id;
    }

    public async ValueTask<bool> Update(int id, LoyaltyProgramUser value)
    {
        var result = await loyaltyProgramUserStore.Update(id, value);
        _ = eventFeed.Raise("User updated", value);
        return result;
    }
}