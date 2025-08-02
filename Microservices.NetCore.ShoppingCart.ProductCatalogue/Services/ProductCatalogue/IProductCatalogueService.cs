using Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue;

public interface IProductCatalogueService
{
    ValueTask<IEnumerable<Product>> GetByIds(params int[] ids);
}