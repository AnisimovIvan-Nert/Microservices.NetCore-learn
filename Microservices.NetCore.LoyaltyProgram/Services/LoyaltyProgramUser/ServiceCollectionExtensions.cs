using Microservices.NetCore.LoyaltyProgram.Services.LoyaltyProgramUser.Store;

namespace Microservices.NetCore.LoyaltyProgram.Services.LoyaltyProgramUser;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUsers(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<ILoyaltyProgramUserService, LoyaltyProgramUserService>()
            .AddScoped<ILoyaltyProgramUserStore, LoyaltyProgramUserStore>();
    }
}