namespace Microservices.NetCore.ShoppingCart.Shared.ProductCatalogue;

public class ProductCatalogueService(IProductStore productStore) : IProductCatalogueService
{
    public ValueTask<IEnumerable<Product>> GetByIds(params int[] ids)
    {
        return productStore.GetByIds(ids);
    }
}