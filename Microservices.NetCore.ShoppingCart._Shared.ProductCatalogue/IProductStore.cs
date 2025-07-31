namespace Microservices.NetCore.ShoppingCart.Shared.ProductCatalogue;

public interface IProductStore
{
    ValueTask<IEnumerable<Product>> GetByIds(params int[] productIds);
}