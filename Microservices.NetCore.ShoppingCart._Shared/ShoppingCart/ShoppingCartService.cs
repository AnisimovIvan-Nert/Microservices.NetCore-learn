using Microservices.NetCore.ShoppingCart._Shared.EventFeed;
using Microservices.NetCore.ShoppingCart._Shared.ProductClient;

namespace Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;

public class ShoppingCartService(
    IShoppingCartStore shoppingCartStore,
    IProductCatalogueClient productCatalogue,
    IEventFeed eventFeed) 
    : IShoppingCartService
{
    public ShoppingCart Get(int id)
    {
        return shoppingCartStore.Get(id);
    }
    
    public async Task<ShoppingCart> PostItems(int id, int[] itemIds)
    {
        var shoppingCart = shoppingCartStore.Get(id);
        var shoppingCartItems = await productCatalogue.GetShoppingCartItems(itemIds);
        shoppingCart.AddItems(shoppingCartItems, eventFeed);
        shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }
    
    public ShoppingCart DeleteItems(int id, int[] itemsIds)
    {
        var shoppingCart = shoppingCartStore.Get(id);
        shoppingCart.RemoveItems(itemsIds);
        shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }
}