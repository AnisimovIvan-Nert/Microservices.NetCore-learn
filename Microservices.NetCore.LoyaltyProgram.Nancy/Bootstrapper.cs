using Microservices.NetCore.LoyaltyProgram.Shared.Users;
using Microservices.NetCore.Shared.Nancy.EventFeed;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Microservices.NetCore.LoyaltyProgram.Nancy;

public class Bootstrapper : DefaultNancyBootstrapper
{
    protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
        => NancyInternalConfiguration.WithOverrides(builder => builder.StatusCodeHandlers.Clear());
    
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);
        
        container.RegisterEventFeed();
        container.Register<IUsersService, UsersService>();
        container.Register<IUsersStore, UsersStore>();
    }
}