using Microservices.NetCore.LoyaltyProgram.Services.LoyaltyProgramUser.Store;
using Microservices.NetCore.Shared.EventFeed;

namespace Microservices.NetCore.LoyaltyProgram.Services.LoyaltyProgramUser;

public class LoyaltyProgramUserService(ILoyaltyProgramUserStore loyaltyProgramUserStore, IEventFeed eventFeed) : ILoyaltyProgramUserService
{
    public ValueTask<IEnumerable<Model.LoyaltyProgramUser>> GetAll()
    {
        return loyaltyProgramUserStore.GetAll();
    }
    
    public ValueTask<Model.LoyaltyProgramUser> Get(int id)
    {
        return loyaltyProgramUserStore.Get(id);
    }
    
    public ValueTask<Model.LoyaltyProgramUser?> TryGet(int id)
    {
        return loyaltyProgramUserStore.TryGet(id);
    }

    public async ValueTask<int> Register(Model.LoyaltyProgramUser user)
    {
        var id = await loyaltyProgramUserStore.Create(user);
        _ = eventFeed.Raise("User registered", user);
        return id;
    }

    public async ValueTask Update(int id, Model.LoyaltyProgramUser value)
    {
        await loyaltyProgramUserStore.Update(id, value);
        _ = eventFeed.Raise("User updated", value);
    }
}