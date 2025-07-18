using Microservices.NetCore.Nancy.Shared;
using Microservices.NetCore.Nancy.ShoppingCart.EventFeed;
using Microservices.NetCore.Nancy.ShoppingCart.ProductClient;
using Nancy;
using Nancy.ModelBinding;

namespace Microservices.NetCore.Nancy.ShoppingCart.ShoppingCart;

public sealed class ShoppingCartModule : NancyModule
{
    private const string ModuleUri = "/shoppingcart";
        
    public ShoppingCartModule(
        IShoppingCartStore shoppingCartStore, 
        IProductCatalogueClient productCatalogue, 
        IEventStore eventStore) 
        : base(ModuleUri)
    {
        const string userIdParameterName = "userid";

        var userIdParameter = new QueryParameter<int>(userIdParameterName, "int");
            
        Get($"/{userIdParameter}", parameters =>
        {
            var userId = (int)userIdParameter.Get(parameters);
            return shoppingCartStore.Get(userId);
        });

        Post($"/{userIdParameter}/items", async (parameters, _) =>
        {
            var productCatalogueIds = this.Bind<int[]>();
            var userId = (int)userIdParameter.Get(parameters);
                
            var shoppingCart = shoppingCartStore.Get(userId);
            var shoppingCartItems = await productCatalogue.GetShoppingCartItems(productCatalogueIds);
            shoppingCart.AddItems(shoppingCartItems, eventStore);
            shoppingCartStore.Save(shoppingCart);
                
            return shoppingCart;
        });

        Delete($"/{userIdParameter}/items", parameters =>
        {
            var productCatalogueIds = this.Bind<int[]>();
            var userId = (int)userIdParameter.Get(parameters);

            var shoppingCart = shoppingCartStore.Get(userId);
            shoppingCart.RemoveItems(productCatalogueIds);
            shoppingCartStore.Save(shoppingCart);

            return shoppingCart;
        });
    }
}