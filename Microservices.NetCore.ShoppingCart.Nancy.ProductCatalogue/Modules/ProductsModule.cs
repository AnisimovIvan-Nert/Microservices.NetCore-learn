using Microservices.NetCore.ShoppingCart.Shared.ProductCatalogue;
using Nancy;

namespace Microservices.NetCore.ShoppingCart.Nancy.ProductCatalogue.Modules;

public sealed class ProductsModule : NancyModule
{
    private const string ModuleUri = "/products";

    public ProductsModule(IProductCatalogueService productCatalogueService) : base(ModuleUri)
    {
        Get("", async _ =>
        {
            var productIdsString = (string)Request.Query.productIds;
            var productIds = productIdsString.Split(',').Select(int.Parse).ToArray();
            var products = await productCatalogueService.GetByIds(productIds);

            return Negotiate
                .WithModel(products)
                .WithHeader("cache-control", "max-age:86400");
        });
    }
}