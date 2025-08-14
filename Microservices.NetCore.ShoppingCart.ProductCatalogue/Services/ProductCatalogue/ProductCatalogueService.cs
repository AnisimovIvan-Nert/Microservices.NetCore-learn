using Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;
using Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue.Store;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue;

public class ProductCatalogueService(IProductStore productStore) : IProductCatalogueService
{
    public ValueTask<IEnumerable<Product>> GetBatch(int batchStart, int batchSize)
    {
        if (batchStart < 0 || batchSize < 0)
            throw new InvalidOperationException();
        
        return productStore.GetBatch(batchStart, batchSize);
    }

    public ValueTask<IEnumerable<Product>> GetByIds(params int[] ids)
    {
        return productStore.GetByIds(ids);
    }
}