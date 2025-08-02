using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.ShoppingCart.Clients.ProductCatalogue;
using Microservices.NetCore.ShoppingCart.Services.ShoppingCart.Store;

namespace Microservices.NetCore.ShoppingCart.Services.ShoppingCart;

public class ShoppingCartService(
    IShoppingCartStore shoppingCartStore,
    IProductCatalogueClient productCatalogue,
    IEventFeed eventFeed) 
    : IShoppingCartService
{
    public ValueTask<Models.ShoppingCart.ShoppingCart> Create()
    {
        return shoppingCartStore.Create();
    }

    public ValueTask<Models.ShoppingCart.ShoppingCart> Get(int id)
    {
        return shoppingCartStore.Get(id);
    }
    
    public async ValueTask<Models.ShoppingCart.ShoppingCart> PostItems(int id, int[] itemIds)
    {
        const string addItemEventName = $"{nameof(Models.ShoppingCart.ShoppingCart)} item added";
        
        var shoppingCart = await shoppingCartStore.Get(id);
        var productCatalogueItems = await productCatalogue.GetProductCatalogueItems(itemIds);
        var addedItems = shoppingCart.AddItems(productCatalogueItems);
        foreach (var addedItem in addedItems)
        {
            await eventFeed.Raise(addItemEventName, addedItem);
        }
        
        await shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }
    
    public async ValueTask<Models.ShoppingCart.ShoppingCart> DeleteItems(int id, int[] itemsIds)
    {
        var shoppingCart = await shoppingCartStore.Get(id);
        shoppingCart.RemoveItems(itemsIds);
        await shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }
}