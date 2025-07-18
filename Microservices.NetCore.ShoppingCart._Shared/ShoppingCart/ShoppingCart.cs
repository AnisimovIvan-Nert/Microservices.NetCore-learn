using Microservices.NetCore.ShoppingCart._Shared.EventFeed;

namespace Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;

public class ShoppingCart(int userId)
{
    private const string AddedEventName = "ShoppingCartItemAdded";
        
    private readonly HashSet<ShoppingCartItem> _items = [];

    public int UserId { get; } = userId;
    public IEnumerable<ShoppingCartItem> Items => _items;

    public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventFeed eventFeed)
    {
        foreach (var item in shoppingCartItems)
        {
            if (_items.Add(item) == false) 
                continue;
                
            var content = new { UserId, item };
            eventFeed.Raise(AddedEventName, content);
        }
    }

    public void RemoveItems(int[] productCatalogueIds)
    {
        _items.RemoveWhere(item => productCatalogueIds.Contains(item.ProductCatalogueId));
    }
}