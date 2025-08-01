using Microservices.NetCore.ShoppingCart.Shared.ProductCatalogue;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.ProductCatalogue.Controllers;

[ApiController]
[Route("products")]
public class ProductCatalogueController(IProductCatalogueService productCatalogueService) : ControllerBase
{
    [HttpGet]
    public ValueTask<IEnumerable<Product>> GetProducts([FromQuery]int[] ids)
    {
        return productCatalogueService.GetByIds(ids);
    }
}