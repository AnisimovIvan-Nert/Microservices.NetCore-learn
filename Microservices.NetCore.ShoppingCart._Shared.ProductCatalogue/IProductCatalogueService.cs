namespace Microservices.NetCore.ShoppingCart.Shared.ProductCatalogue;

public interface IProductCatalogueService
{
    ValueTask<IEnumerable<Product>> GetByIds(params int[] ids);
}