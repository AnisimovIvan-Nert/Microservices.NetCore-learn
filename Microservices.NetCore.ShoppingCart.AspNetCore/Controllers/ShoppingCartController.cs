using Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Controllers;

[ApiController]
[Route("shoppingcart")]
public class ShoppingCartController(IShoppingCartService shoppingCartService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public _Shared.ShoppingCart.ShoppingCart GetShoppingCart(int id)
    {
        return shoppingCartService.Get(id);
    }
    
    [HttpPost("{id:int}/items")]
    public async Task<_Shared.ShoppingCart.ShoppingCart> PostItemsToCart(int id, int[] itemIds)
    {
        return await shoppingCartService.PostItems(id, itemIds);
    }

    [HttpDelete("{id:int}/items")]
    public _Shared.ShoppingCart.ShoppingCart DeleteItemsFromCart(int id, int[] itemsIds)
    {
        return shoppingCartService.DeleteItems(id, itemsIds);
    }
}