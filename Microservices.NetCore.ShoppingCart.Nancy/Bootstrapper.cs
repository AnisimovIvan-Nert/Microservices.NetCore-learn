using Microservices.NetCore.Shared.Nancy.EventFeed;
using Microservices.NetCore.ShoppingCart.Shared.ProductClient;
using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;
using Nancy;
using Nancy.Configuration;
using Nancy.TinyIoc;

namespace Microservices.NetCore.ShoppingCart.Nancy;

public class Bootstrapper : DefaultNancyBootstrapper
{
    public override void Configure(INancyEnvironment environment)
    {
        environment.Tracing(true, true);            
    }
    
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        container.RegisterEventFeed();
        container.Register<IShoppingCartService, ShoppingCartService>();
        container.Register<IShoppingCartStore, InMemoryShoppingCartStore>();
        container.Register<IProductCatalogueClient, ProductCatalogueClient>();
    }
}