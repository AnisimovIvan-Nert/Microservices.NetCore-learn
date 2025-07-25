using Microservices.NetCore.LoyaltyProgram.Shared.Users;
using Microservices.NetCore.Shared.AspNetCore.EventFeed;

namespace Microservices.NetCore.LoyaltyProgram.AspNetCore.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoyaltyProgramServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddEventFeed()
            .AddUsers();
    }
    
    private static IServiceCollection AddUsers(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IUsersService, UsersService>()
            .AddScoped<IUsersStore, UsersStore>();
    }
}