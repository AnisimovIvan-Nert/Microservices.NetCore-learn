using System.Text.Json;
using Microservices.NetCore.Shared.Cache;
using Polly;
using Polly.Retry;

namespace Microservices.NetCore.ShoppingCart.Shared.ProductClient;

public class ProductCatalogueClient(ICacheStore cacheStore) : IProductCatalogueClient
{
    //TODO link uri
    private const string ProductCatalogueBaseUri = "http://localhost:5013";
    private const string ProductPathTemplate = "/products?{0}";
    private const string IdsParameterTemplate = "ids={0}";

    public ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItems(params int[] productCatalogueIds)
    {
        var retryPolicy = CreateRetryPolicy();
        var task = retryPolicy.ExecuteAsync(() => GetItemsFromCatalogueService(productCatalogueIds));
        return new ValueTask<IEnumerable<ProductCatalogueItem>>(task);
    }

    private async Task<IEnumerable<ProductCatalogueItem>> GetItemsFromCatalogueService(int[] productCatalogueIds)
    {
        var response = await RequestProductFromProductCatalogue(productCatalogueIds);
        return await ConvertToShoppingCartItems(response);
    }

    private async ValueTask<HttpResponseMessage> RequestProductFromProductCatalogue(int[] productCatalogueIds)
    {
        var idsParams = productCatalogueIds.Select(id => string.Format(IdsParameterTemplate, id));
        var comaSeparatedProductCatalogueIds = string.Join("&", idsParams);
        var productsResource = string.Format(ProductPathTemplate, comaSeparatedProductCatalogueIds);

        if (cacheStore.TryGet(productsResource) is HttpResponseMessage cachedResponse)
            return cachedResponse;

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(ProductCatalogueBaseUri);
        var response = await httpClient.GetAsync(productsResource);
        
        response.EnsureSuccessStatusCode();
        CacheResponse(productsResource, response);
        return response;
    }

    private static async ValueTask<IEnumerable<ProductCatalogueItem>> ConvertToShoppingCartItems(
        HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ProductCatalogueItem>>(responseContent, JsonSerializerOptions.Web) ?? [];
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