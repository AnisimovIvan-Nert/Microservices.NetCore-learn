using Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue.Store;

public interface IProductStore
{
    ValueTask<IEnumerable<Product>> GetByIds(params int[] productIds);
}