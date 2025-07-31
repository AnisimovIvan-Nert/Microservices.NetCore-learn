using System.Text.Json;
using Microservices.NetCore.Shared.Cache;
using Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;
using Polly;
using Polly.Retry;

namespace Microservices.NetCore.ShoppingCart.Shared.ProductClient;

public class ProductCatalogueClient(ICacheStore cacheStore) : IProductCatalogueClient
{
    //TODO link uri
    private const string ProductCatalogueBaseUri = "http://localhost:5267";
    private const string ProductPathTemplate = "/products?productIds={0}";

    public ValueTask<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(params int[] productCatalogueIds)
    {
        var retryPolicy = CreateRetryPolicy();
        var task = retryPolicy.ExecuteAsync(async () => await GetItemsFromCatalogueService(productCatalogueIds));
        return new ValueTask<IEnumerable<ShoppingCartItem>>(task);
    }

    private async ValueTask<IEnumerable<ShoppingCartItem>> GetItemsFromCatalogueService(int[] productCatalogueIds)
    {
        var response = await RequestProductFromProductCatalogue(productCatalogueIds);
        return await ConvertToShoppingCartItems(response);
    }

    private async ValueTask<HttpResponseMessage> RequestProductFromProductCatalogue(int[] productCatalogueIds)
    {
        var comaSeparatedProductCatalogueIds = string.Join(",", productCatalogueIds);
        var productsResource = string.Format(ProductPathTemplate, comaSeparatedProductCatalogueIds);

        if (cacheStore.TryGet(productsResource) is HttpResponseMessage cachedResponse)
            return cachedResponse;

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(ProductCatalogueBaseUri);
        var response = await httpClient.GetAsync(productsResource);
        CacheResponse(productsResource, response);
        return response;
    }

    private static async ValueTask<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(
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
    
    private void CacheResponse(string resource, HttpResponseMessage response)
    {
        var maxAge = response.Headers.CacheControl?.MaxAge;
        if (maxAge != null)
            cacheStore.Add(resource, response, maxAge.Value);
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