using Microservices.NetCore.LoyaltyProgram.Shared.Users;
using Microservices.NetCore.Shared.Nancy;
using Nancy;
using Nancy.ModelBinding;

namespace Microservices.NetCore.LoyaltyProgram.Nancy;

public sealed class UsersModule : NancyModule
{
    private const string ModuleUri = "/users";

    public UsersModule(IUsersService usersService) : base(ModuleUri)
    {
        const string userIdParameterName = "userid";
        var userIdParameter = new QueryParameter<int>(userIdParameterName, "int");
        
        Get("/", async _ => await usersService.GetAll());
        
        Get($"/{userIdParameter}", async parameters =>
        {
            int userId = userIdParameter.Get(parameters);

            var user = await usersService.TryGet(userId);
            return user != null 
                ? user 
                : HttpStatusCode.NotFound;
        });
        
        Post("/", async _ =>
        {
            var newUser = this.Bind<LoyaltyProgramUser>();

            await usersService.Register(newUser);
            
            return Negotiate
                .WithStatusCode(HttpStatusCode.Created)
                .WithHeader("Location", Request.Url.SiteBase + "/users/" + newUser.Id)
                .WithModel(newUser);
        });
        
        Put($"/{userIdParameter}", async parameters =>
        {
            int userId = userIdParameter.Get(parameters);
            var updatedUser = this.Bind<LoyaltyProgramUser>();

            await usersService.Update(userId, updatedUser);
            return updatedUser;
        });
    }
}