using Microservices.NetCore.LoyaltyProgram.Model;

namespace Microservices.NetCore.LoyaltyProgram.Services.User.Store;

public interface ILoyaltyProgramUserStore
{
    ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll();
    ValueTask<LoyaltyProgramUser> Get(int id);
    ValueTask<LoyaltyProgramUser?> TryGet(int id);
    ValueTask<int> Create(LoyaltyProgramUser user);
    ValueTask<bool> Update(int id, LoyaltyProgramUser newUser);
}