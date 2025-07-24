using Microservices.NetCore.ShoppingCart._Shared.EventFeed;
using Microservices.NetCore.ShoppingCart._Shared.ProductClient;
using Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;
using Nancy;
using Nancy.TinyIoc;

namespace Microservices.NetCore.ShoppingCart.Nancy;

public class Bootstrapper : DefaultNancyBootstrapper
{
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);
        
        container.Register<IEventStore, EventStore>();
        container.Register<IEventFeed, EventFeed>();
        container.Register<IShoppingCartService, ShoppingCartService>();
        container.Register<IShoppingCartStore, ShoppingCartStore>();
        container.Register<IProductCatalogueClient, InMemoryProductCatalogueClient>();
    }
}