using Microservices.NetCore.LoyaltyProgram.Model;

namespace Microservices.NetCore.LoyaltyProgram.Services.User;

public interface ILoyaltyProgramUserService
{
    ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll();
    ValueTask<LoyaltyProgramUser> Get(int id);
    ValueTask<LoyaltyProgramUser?> TryGet(int id);
    ValueTask<int> Register(LoyaltyProgramUser user);
    ValueTask<bool> Update(int id, LoyaltyProgramUser value);
}