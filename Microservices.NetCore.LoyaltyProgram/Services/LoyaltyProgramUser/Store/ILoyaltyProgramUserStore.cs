namespace Microservices.NetCore.LoyaltyProgram.Services.LoyaltyProgramUser.Store;

public interface ILoyaltyProgramUserStore
{
    ValueTask<IEnumerable<Model.LoyaltyProgramUser>> GetAll();
    ValueTask<Model.LoyaltyProgramUser> Get(int id);
    ValueTask<Model.LoyaltyProgramUser?> TryGet(int id);
    ValueTask<int> Create(Model.LoyaltyProgramUser user);
    ValueTask<bool> Update(int id, Model.LoyaltyProgramUser value);
}