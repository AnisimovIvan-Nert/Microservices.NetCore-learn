using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Microservices.NetCore.ShoppingCart.Tests;

public class ProductCatalogueApplicationFactory : WebApplicationFactory<ProductCatalogue.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Development);
    }
}