using Microservices.NetCore.ShoppingCart.Shared.ProductCatalogue;
using Nancy;
using Nancy.Configuration;
using Nancy.TinyIoc;

namespace Microservices.NetCore.ShoppingCart.Nancy.ProductCatalogue;

public class Bootstrapper : DefaultNancyBootstrapper
{
    public override void Configure(INancyEnvironment environment)
    {
        environment.Tracing(true, true);            
    }
    
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);
        
        container.Register<IProductCatalogueService, ProductCatalogueService>();
        container.Register<IProductStore, ProductStore>();
    }
}