using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.ShoppingCart.Shared.ProductClient;

namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public class ShoppingCartService(
    IShoppingCartStore shoppingCartStore,
    IProductCatalogueClient productCatalogue,
    IEventFeed eventFeed) 
    : IShoppingCartService
{
    public ValueTask<ShoppingCart> Create()
    {
        return shoppingCartStore.Create();
    }

    public ValueTask<ShoppingCart> Get(int id)
    {
        return shoppingCartStore.Get(id);
    }
    
    public async ValueTask<ShoppingCart> PostItems(int id, int[] itemIds)
    {
        var shoppingCart = await shoppingCartStore.Get(id);
        var productCatalogueItems = await productCatalogue.GetProductCatalogueItems(itemIds);
        shoppingCart.AddItems(productCatalogueItems, eventFeed);
        await shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }
    
    public async ValueTask<ShoppingCart> DeleteItems(int id, int[] itemsIds)
    {
        var shoppingCart = await shoppingCartStore.Get(id);
        shoppingCart.RemoveItems(itemsIds);
        await shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }
}