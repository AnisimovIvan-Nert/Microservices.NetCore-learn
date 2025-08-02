using Microservices.NetCore.ShoppingCart.Models.ProductCatalogue;

namespace Microservices.NetCore.ShoppingCart.Models.ShoppingCart;

public class ShoppingCart(int id, params ShoppingCartItem[] items)
{
    private readonly HashSet<ShoppingCartItem> _items = items.ToHashSet();

    public int Id { get; } = id;
    public IEnumerable<ShoppingCartItem> Items => _items;

    public IEnumerable<ShoppingCartItem> AddItems(IEnumerable<ProductCatalogueItem> productCatalogueItems)
    {
        foreach (var item in productCatalogueItems)
        {
            var stopingCartItem = new ShoppingCartItem(id, item);
            
            if (_items.Add(stopingCartItem)) 
                yield return stopingCartItem;
        }
    }

    public void RemoveItems(int[] productCatalogueIds)
    {
        _items.RemoveWhere(item => productCatalogueIds.Contains(item.ProductId));
    }
}