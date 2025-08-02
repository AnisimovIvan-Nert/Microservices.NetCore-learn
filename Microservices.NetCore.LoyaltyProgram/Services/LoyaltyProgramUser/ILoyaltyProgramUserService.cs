namespace Microservices.NetCore.LoyaltyProgram.Services.LoyaltyProgramUser;

public interface ILoyaltyProgramUserService
{
    ValueTask<IEnumerable<Model.LoyaltyProgramUser>> GetAll();
    ValueTask<Model.LoyaltyProgramUser> Get(int id);
    ValueTask<Model.LoyaltyProgramUser?> TryGet(int id);
    ValueTask<int> Register(Model.LoyaltyProgramUser user);
    ValueTask Update(int id, Model.LoyaltyProgramUser value);
}