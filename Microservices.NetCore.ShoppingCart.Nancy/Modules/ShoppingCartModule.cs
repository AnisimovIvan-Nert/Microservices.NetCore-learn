using Microservices.NetCore.Shared.Nancy;
using Microservices.NetCore.ShoppingCart._Shared.ShoppingCart;
using Nancy;
using Nancy.ModelBinding;

namespace Microservices.NetCore.ShoppingCart.Nancy.Modules;

public sealed class ShoppingCartModule : NancyModule
{
    private const string ModuleUri = "/shoppingcart";
        
    public ShoppingCartModule(IShoppingCartService shoppingCartService) : base(ModuleUri)
    {
        const string userIdParameterName = "userid";
        var userIdParameter = new QueryParameter<int>(userIdParameterName, "int");
            
        Get($"/{userIdParameter}", parameters =>
        {
            int userId = userIdParameter.Get(parameters);
            
            return shoppingCartService.Get(userId);
        });

        Post($"/{userIdParameter}/items", async (parameters, _) =>
        {
            var productCatalogueIds = this.Bind<int[]>();
            int userId = userIdParameter.Get(parameters);
                
            return await shoppingCartService.PostItems(userId, productCatalogueIds);
        });

        Delete($"/{userIdParameter}/items", parameters =>
        {
            var productCatalogueIds = this.Bind<int[]>();
            int userId = userIdParameter.Get(parameters);

            return shoppingCartService.DeleteItems(userId, productCatalogueIds);
        });
    }
}