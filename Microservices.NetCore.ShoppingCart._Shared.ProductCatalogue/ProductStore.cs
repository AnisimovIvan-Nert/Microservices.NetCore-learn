namespace Microservices.NetCore.ShoppingCart.Shared.ProductCatalogue;

public class ProductStore : IProductStore
{
    public ValueTask<IEnumerable<Product>> GetByIds(params int[] productIds)
    {
        return ValueTask.FromResult(productIds.Select(id => new Product(id, "name" + id, "description", new Money("USD", 123))));
    }
}