using Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Services.ProductCatalogue.Store;

public class ProductStore : IProductStore
{
    public ValueTask<IEnumerable<Product>> GetBatch(int batchStart, int batchSize)
    {
        var batchRange = Enumerable.Range(batchStart, batchSize);
        return GetByIds(batchRange.ToArray());
    }

    public ValueTask<IEnumerable<Product>> GetByIds(params int[] productIds)
    {
        return ValueTask.FromResult(productIds.Select(id => new Product(id, "name" + id, "description", new Money("USD", 123))));
    }
}