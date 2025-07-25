using Microservices.NetCore.Shared.Nancy.EventFeed;
using Microservices.NetCore.ShoppingCart.Shared.ProductClient;
using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;
using Nancy;
using Nancy.TinyIoc;

namespace Microservices.NetCore.ShoppingCart.Nancy;

public class Bootstrapper : DefaultNancyBootstrapper
{
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        container.RegisterEventFeed();
        container.Register<IShoppingCartService, ShoppingCartService>();
        container.Register<IShoppingCartStore, ShoppingCartStore>();
        container.Register<IProductCatalogueClient, InMemoryProductCatalogueClient>();
    }
}