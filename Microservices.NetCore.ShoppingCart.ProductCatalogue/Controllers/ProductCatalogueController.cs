using Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;
using Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Controllers;

[ApiController]
[Route("products")]
public class ProductCatalogueController(IProductCatalogueService productCatalogueService) : ControllerBase
{
    [HttpGet("batch")]
    public async ValueTask<IActionResult> GetProducts(int batchStart = 0, int batchEnd = int.MaxValue)
    {
        var batchSize = batchEnd - batchStart;

        if (batchStart < 0 || batchSize < 0)
            return BadRequest();
        
        var products = await productCatalogueService.GetBatch(batchStart, batchSize);
        return Ok(products);
    }
    
    [HttpGet]
    public ValueTask<IEnumerable<Product>> GetProducts([FromQuery]int[] ids)
    {
        return productCatalogueService.GetByIds(ids);
    }
}