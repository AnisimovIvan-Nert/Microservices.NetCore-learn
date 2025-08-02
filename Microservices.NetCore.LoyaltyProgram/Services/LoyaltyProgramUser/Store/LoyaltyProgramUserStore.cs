namespace Microservices.NetCore.LoyaltyProgram.Services.LoyaltyProgramUser.Store;

public class LoyaltyProgramUserStore : ILoyaltyProgramUserStore
{
    private static readonly List<Model.LoyaltyProgramUser> _users = [];

    public ValueTask<IEnumerable<Model.LoyaltyProgramUser>> GetAll()
    {
        var result = _users.AsEnumerable();
        return ValueTask.FromResult(result);
    }

    public ValueTask<Model.LoyaltyProgramUser> Get(int id)
    {
        var result = _users.Single(user => user.Id == id);
        return ValueTask.FromResult(result);
    }

    public ValueTask<Model.LoyaltyProgramUser?> TryGet(int id)
    {
        var result = _users.SingleOrDefault(user => user.Id == id);
        return ValueTask.FromResult(result);
    }

    public ValueTask<int> Create(Model.LoyaltyProgramUser user)
    {
        var lastId = _users.LastOrDefault()?.Id ?? -1;
        var id = lastId + 1;
        user.Id = id;
        
        _users.Add(user);
        return ValueTask.FromResult(id);
    }

    public ValueTask<bool> Update(int id, Model.LoyaltyProgramUser value)
    {
        var index = _users.FindIndex(user => user.Id == id);
        value.Id = id;
        _users[index] = value;
        return ValueTask.FromResult(true);
    }
}