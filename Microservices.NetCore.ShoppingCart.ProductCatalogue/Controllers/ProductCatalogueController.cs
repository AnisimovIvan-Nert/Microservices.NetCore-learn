using Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;
using Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Controllers;

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