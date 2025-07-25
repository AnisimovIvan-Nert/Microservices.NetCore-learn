using System.Text.Json;
using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;
using Polly;
using Polly.Retry;

namespace Microservices.NetCore.ShoppingCart.Shared.ProductClient;

public class MemoryProductCatalogueClient : IProductCatalogueClient
{
    //TODO link uri
    private const string ProductCatalogueBaseUri = "";
    private const string ProductPathTemplate = "/products?productIds=[{0}]";

    public Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds)
    {
        var retryPolicy = CreateRetryPolicy();
        return retryPolicy.ExecuteAsync(async () => await GetItemsFromCatalogueService(productCatalogueIds));
    }

    private static async Task<IEnumerable<ShoppingCartItem>> GetItemsFromCatalogueService(int[] productCatalogueIds)
    {
        var response = await RequestProductFromProductCatalogue(productCatalogueIds);
        return await ConvertToShoppingCartItems(response);
    }

    private static async Task<HttpResponseMessage> RequestProductFromProductCatalogue(int[] productCatalogueIds)
    {
        var comaSeparatedProductCatalogueIds = string.Join(",", productCatalogueIds);
        var productsResource = string.Format(ProductPathTemplate, comaSeparatedProductCatalogueIds);

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(ProductCatalogueBaseUri);
        return await httpClient.GetAsync(productsResource);
    }

    private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(
        HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<List<ProductCatalogueItem>>(responseContent) ?? [];
        return products.Select(ConvertToShoppingCartItem);
    }

    private static ShoppingCartItem ConvertToShoppingCartItem(ProductCatalogueItem item)
    {
        var id = int.Parse(item.ProductId);
        return new ShoppingCartItem(id, item.ProductName, item.ProductDescription, item.Price);
    }

    private static AsyncRetryPolicy CreateRetryPolicy()
    {
        const int retryCount = 3;
        const int baseSleepDuration = 100;

        return Policy.Handle<Exception>()
            .WaitAndRetryAsync(retryCount, SleepDurationProvider, OnRetry);

        TimeSpan SleepDurationProvider(int attempt)
        {
            return TimeSpan.FromMilliseconds(baseSleepDuration * Math.Pow(2, attempt));
        }

        void OnRetry(Exception exception, TimeSpan _)
        {
            Console.WriteLine(exception.ToString());
        }
    }
}