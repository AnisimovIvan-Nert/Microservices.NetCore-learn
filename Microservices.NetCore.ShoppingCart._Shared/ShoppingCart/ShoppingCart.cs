using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.ShoppingCart.Shared.ProductClient;

namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public class ShoppingCart(int id, params ShoppingCartItem[] items)
{
    private const string AddedEventName = "ShoppingCartItemAdded";
        
    private readonly HashSet<ShoppingCartItem> _items = items.ToHashSet();

    public int Id { get; } = id;
    public IEnumerable<ShoppingCartItem> Items => _items;

    public void AddItems(IEnumerable<ProductCatalogueItem> productCatalogueItems, IEventFeed eventFeed)
    {
        foreach (var item in productCatalogueItems)
        {
            var stopingCartItem = new ShoppingCartItem(id, item);
            
            if (_items.Add(stopingCartItem) == false) 
                continue;
                
            var content = new { Id, stopingCartItem };
            _ = eventFeed.Raise(AddedEventName, content);
        }
    }

    public void RemoveItems(int[] productCatalogueIds)
    {
        _items.RemoveWhere(item => productCatalogueIds.Contains(item.ProductId));
    }
}