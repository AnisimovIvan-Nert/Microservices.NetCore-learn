using Microservices.NetCore.LoyaltyProgram.Model;
using Microservices.NetCore.Shared.Store;
using Microservices.NetCore.Shared.ValueGenerators;

namespace Microservices.NetCore.LoyaltyProgram.Services.User.Store;

public class InMemoryLoyaltyProgramUserStore : ILoyaltyProgramUserStore
{
    private readonly IStore<int, LoyaltyProgramUser> _store;
    private readonly IValueGenerator<int> _idGenerator;

    public InMemoryLoyaltyProgramUserStore(IStoreSource storeSource)
    {
        _store = storeSource.GetStore<int, LoyaltyProgramUser>(nameof(InMemoryLoyaltyProgramUserStore));
        var generators = storeSource.GetOrLinkGenerators(_store, CreateGenerators);
        _idGenerator = (IValueGenerator<int>)generators.Single();
        return;

        IReadOnlyCollection<IValueGenerator> CreateGenerators()
        {
            return [new IntGenerator()];
        }
    }

    public void Clear()
    {
        _store.Clear();
    }
    
    public ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll()
    {
        var result = _store.AsEnumerable();
        return ValueTask.FromResult(result);
    }

    public ValueTask<LoyaltyProgramUser> Get(int id)
    {
        var user = _store.Get(id);

        if (user == null)
            throw new InvalidOperationException();
        
        return ValueTask.FromResult(user);
    }

    public ValueTask<LoyaltyProgramUser?> TryGet(int id)
    {
        var user = _store.Get(id);
        return ValueTask.FromResult(user);
    }

    public ValueTask<int> Create(LoyaltyProgramUser user)
    {
        if (_store.Contains(user))
            throw new InvalidOperationException();

        user.Id = _idGenerator.GenerateNext();
        return _store.Add(user.Id, user) 
            ? ValueTask.FromResult(user.Id) 
            : throw new InvalidOperationException();
    }

    public ValueTask<bool> Update(int id, LoyaltyProgramUser newUser)
    {
        var oldUser = _store.Get(id);

        if (oldUser == null)
            return ValueTask.FromResult(false);
        
        newUser.Id = id;
        return _store.Update(id, newUser, oldUser)
            ? ValueTask.FromResult(true)
            : throw new InvalidOperationException();
    }
}