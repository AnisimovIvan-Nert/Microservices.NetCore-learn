namespace Microservices.NetCore.LoyaltyProgram.Shared.Users;

public class UsersStore : IUsersStore
{
    private static readonly List<LoyaltyProgramUser> _users = [];

    public ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll()
    {
        var result = _users.AsEnumerable();
        return ValueTask.FromResult(result);
    }

    public ValueTask<LoyaltyProgramUser> Get(int id)
    {
        var result = _users.Single(user => user.Id == id);
        return ValueTask.FromResult(result);
    }

    public ValueTask<LoyaltyProgramUser?> TryGet(int id)
    {
        var result = _users.SingleOrDefault(user => user.Id == id);
        return ValueTask.FromResult(result);
    }

    public ValueTask<int> Create(LoyaltyProgramUser user)
    {
        var lastId = _users.LastOrDefault()?.Id ?? -1;
        var id = lastId + 1;
        user.Id = id;
        
        _users.Add(user);
        return ValueTask.FromResult(id);
    }

    public ValueTask<bool> Update(int id, LoyaltyProgramUser value)
    {
        var index = _users.FindIndex(user => user.Id == id);
        value.Id = id;
        _users[index] = value;
        return ValueTask.FromResult(true);
    }
}