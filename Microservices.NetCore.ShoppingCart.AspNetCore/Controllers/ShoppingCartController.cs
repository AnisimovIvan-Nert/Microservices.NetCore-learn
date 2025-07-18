using Microservices.NetCore.ShoppingCart._Shared.EventFeed;
using Microservices.NetCore.ShoppingCart._Shared.ProductClient;
using Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Controllers;

[ApiController]
[Route("shoppingcart")]
public class ShoppingCartController(
    IShoppingCartStore shoppingCartStore, 
    IProductCatalogueClient productCatalogue,
    IEventStore eventStore) 
    : ControllerBase
{
    [HttpGet("{id:int}")]
    public _Shared.ShoppingCart.ShoppingCart GetShoppingCart(int id)
    {
        return shoppingCartStore.Get(id);
    }
    
    [HttpPost("{id:int}/items")]
    public async Task<_Shared.ShoppingCart.ShoppingCart> PostItemsToCart(int id, int[] itemIds)
    {
        var shoppingCart = shoppingCartStore.Get(id);
        var shoppingCartItems = await productCatalogue.GetShoppingCartItems(itemIds);
        shoppingCart.AddItems(shoppingCartItems, eventStore);
        shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }

    [HttpDelete("{id:int}/items")]
    public _Shared.ShoppingCart.ShoppingCart DeleteItemsFromCart(int id, int[] itemsIds)
    {
        var shoppingCart = shoppingCartStore.Get(id);
        shoppingCart.RemoveItems(itemsIds);
        shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }
}