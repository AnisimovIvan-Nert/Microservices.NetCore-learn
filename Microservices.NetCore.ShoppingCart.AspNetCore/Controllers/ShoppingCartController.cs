using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Controllers;

[ApiController]
[Route("shoppingcart")]
public class ShoppingCartController(IShoppingCartService shoppingCartService) : ControllerBase
{
    [HttpPost]
    public ValueTask<Shared.ShoppingCart.ShoppingCart> CreateShoppingCart()
    {
        return shoppingCartService.Create();
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public ValueTask<Shared.ShoppingCart.ShoppingCart> GetShoppingCart(int id)
    {
        return shoppingCartService.Get(id);
    }
    
    [HttpPost("{id:int}/items")]
    public ValueTask<Shared.ShoppingCart.ShoppingCart> PostItemsToCart(int id, int[] itemIds)
    {
        return shoppingCartService.PostItems(id, itemIds);
    }

    [HttpDelete("{id:int}/items")]
    public ValueTask<Shared.ShoppingCart.ShoppingCart> DeleteItemsFromCart(int id, int[] itemsIds)
    {
        return shoppingCartService.DeleteItems(id, itemsIds);
    }
}