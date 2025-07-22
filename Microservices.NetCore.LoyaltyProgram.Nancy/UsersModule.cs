using Microservices.NetCore.Shared.Nancy;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;

namespace Microservices.NetCore.LoyaltyProgram.Nancy;

public sealed class UsersModule : NancyModule
{
    private const string ModuleUri = "/users";

    private static readonly IDictionary<int, LoyaltyProgramUser> _registeredUsers =
        new Dictionary<int, LoyaltyProgramUser>();

    public UsersModule() : base(ModuleUri)
    {
        const string userIdParameterName = "userid";
        var userIdParameter = new QueryParameter<int>(userIdParameterName, "int");
        
        Get("/", _ => _registeredUsers.Values);
        
        Get($"/{userIdParameter}", parameters =>
        {
            int userId = userIdParameter.Get(parameters);
            
            if (_registeredUsers.TryGetValue(userId, out var value))
                return value;
            
            return HttpStatusCode.NotFound;
        });
        
        Post("/", _ =>
        {
            var newUser = this.Bind<LoyaltyProgramUser>();
            AddRegisteredUser(newUser);
            return CreatedResponse(newUser);
        });
        
        Put($"/{userIdParameter}", parameters =>
        {
            int userId = userIdParameter.Get(parameters);
            var updatedUser = this.Bind<LoyaltyProgramUser>();
            _registeredUsers[userId] = updatedUser;
            return updatedUser;
        });
    }

    private Negotiator CreatedResponse(LoyaltyProgramUser newUser)
    {
        return Negotiate
                .WithStatusCode(HttpStatusCode.Created)
                .WithHeader("Location", Request.Url.SiteBase + "/users/" + newUser.Id)
                .WithModel(newUser);
    }

    private static void AddRegisteredUser(LoyaltyProgramUser newUser)
    {
        var userId = _registeredUsers.Count;
        newUser.Id = userId;
        _registeredUsers[userId] = newUser;
    }
}

public class LoyaltyProgramUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int LoyaltyPoints { get; set; }
    public LoyaltyProgramSettings Settings { get; set; }
}

public class LoyaltyProgramSettings
{
    public string[] Interests { get; set; }
}