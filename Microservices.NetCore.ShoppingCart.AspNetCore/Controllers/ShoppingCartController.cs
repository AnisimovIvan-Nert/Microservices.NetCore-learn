using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Controllers;

[ApiController]
[Route("shoppingcart/{id:int}")]
public class ShoppingCartController(IShoppingCartService shoppingCartService) : ControllerBase
{
    [HttpGet]
    public ValueTask<Shared.ShoppingCart.ShoppingCart> GetShoppingCart(int id)
    {
        return shoppingCartService.Get(id);
    }
    
    [HttpPost("items")]
    public ValueTask<Shared.ShoppingCart.ShoppingCart> PostItemsToCart(int id, int[] itemIds)
    {
        return shoppingCartService.PostItems(id, itemIds);
    }

    [HttpDelete("items")]
    public ValueTask<Shared.ShoppingCart.ShoppingCart> DeleteItemsFromCart(int id, int[] itemsIds)
    {
        return shoppingCartService.DeleteItems(id, itemsIds);
    }
}