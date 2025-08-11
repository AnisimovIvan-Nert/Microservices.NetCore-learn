using Microservices.NetCore.LoyaltyProgram.Services.User.Store;

namespace Microservices.NetCore.LoyaltyProgram.Services.User;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUsers(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<ILoyaltyProgramUserService, LoyaltyProgramUserService>()
            .AddScoped<ILoyaltyProgramUserStore, LoyaltyProgramUserStore>();
    }
}