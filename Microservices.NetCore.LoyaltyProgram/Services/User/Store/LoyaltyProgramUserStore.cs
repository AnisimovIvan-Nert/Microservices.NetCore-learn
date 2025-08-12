using Microservices.NetCore.LoyaltyProgram.Model;

namespace Microservices.NetCore.LoyaltyProgram.Services.User.Store;

public class LoyaltyProgramUserStore : ILoyaltyProgramUserStore
{
    private static readonly List<LoyaltyProgramUser> Users = [];

    public static void Clear()
    {
        Users.Clear();
    }
    
    public ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll()
    {
        var result = Users.AsEnumerable();
        return ValueTask.FromResult(result);
    }

    public ValueTask<LoyaltyProgramUser> Get(int id)
    {
        var result = Users.Single(user => user.Id == id);
        return ValueTask.FromResult(result);
    }

    public ValueTask<LoyaltyProgramUser?> TryGet(int id)
    {
        var result = FindById(id, out _);
        return ValueTask.FromResult(result);
    }

    public ValueTask<int> Create(LoyaltyProgramUser user)
    {
        if (Users.Contains(user))
            throw new InvalidOperationException();
        
        user.Id = GetNextId();
        Users.Add(user);
        return ValueTask.FromResult(user.Id);
    }

    public ValueTask<bool> Update(int id, LoyaltyProgramUser value)
    {
        var user = FindById(id, out var index);

        if (user == null)
            return ValueTask.FromResult(false);
        
        value.Id = id;
        Users[index] = value;
        return ValueTask.FromResult(true);
    }
    
    private static LoyaltyProgramUser? FindById(int id, out int index)
    {
        index = Users.FindIndex(user => user.Id == id);
        return index == -1 
            ? null 
            : Users[index];
    }
    
    private static int GetNextId()
    {
        var lastId = Users.LastOrDefault()?.Id ?? 0;
        return lastId + 1;
    }
}