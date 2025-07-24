namespace Microservices.NetCore.LoyaltyProgram.Shared.Users;

public interface IUsersStore
{
    ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll();
    ValueTask<LoyaltyProgramUser> Get(int id);
    ValueTask<LoyaltyProgramUser?> TryGet(int id);
    ValueTask<int> Create(LoyaltyProgramUser user);
    ValueTask<bool> Update(int id, LoyaltyProgramUser value);
}