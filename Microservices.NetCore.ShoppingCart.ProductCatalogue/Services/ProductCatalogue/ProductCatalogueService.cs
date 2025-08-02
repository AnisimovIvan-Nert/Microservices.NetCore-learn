using Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;
using Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue.Store;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue;

public class ProductCatalogueService(IProductStore productStore) : IProductCatalogueService
{
    public ValueTask<IEnumerable<Product>> GetByIds(params int[] ids)
    {
        return productStore.GetByIds(ids);
    }
}