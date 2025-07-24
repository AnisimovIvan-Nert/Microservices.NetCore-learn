namespace Microservices.NetCore.LoyaltyProgram.Shared.Users;

public interface IUsersService
{
    ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll();
    ValueTask<LoyaltyProgramUser> Get(int id);
    ValueTask<LoyaltyProgramUser?> TryGet(int id);
    ValueTask<int> Register(LoyaltyProgramUser user);
    ValueTask Update(int id, LoyaltyProgramUser value);
}