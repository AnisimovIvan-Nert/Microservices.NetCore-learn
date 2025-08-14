using System.Text.Json;
using Microservices.NetCore.Shared.Cache;
using Microservices.NetCore.ShoppingCart.Models.ProductCatalogue;
using Microsoft.AspNetCore.Http.Extensions;
using Polly;
using Polly.Retry;

namespace Microservices.NetCore.ShoppingCart.Clients.ProductCatalogue;

public class ProductCatalogueClient(ICacheStore cacheStore) : IProductCatalogueClient
{
    //TODO link uri
    private const string ProductCatalogueBaseUri = "http://localhost:5013";
    private const string ProductUri = "products";
    private const string IdsParameter = "ids";
    private const string ProductsBatchUri = ProductUri + "/batch";
    private const string BatchStartParameter = "batchStart";
    private const string BatchEndParameter = "batchEnd";

    public ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItemsBatch(int start, int size)
    {
        var retryPolicy = CreateRetryPolicy();
        var task = retryPolicy.ExecuteAsync(() => GetItemsBatchFromCatalogueService(start, size));
        return new ValueTask<IEnumerable<ProductCatalogueItem>>(task);
    }

    public ValueTask<IEnumerable<ProductCatalogueItem>> GetProductCatalogueItems(params int[] productCatalogueIds)
    {
        var retryPolicy = CreateRetryPolicy();
        var task = retryPolicy.ExecuteAsync(() => GetItemsFromCatalogueService(productCatalogueIds));
        return new ValueTask<IEnumerable<ProductCatalogueItem>>(task);
    }

    private async Task<IEnumerable<ProductCatalogueItem>> GetItemsBatchFromCatalogueService(int start, int size)
    {
        var response = await RequestProductsBatchFromProductCatalogue(start, size);
        return await ConvertToShoppingCartItems(response);
    }
    
    private async Task<IEnumerable<ProductCatalogueItem>> GetItemsFromCatalogueService(int[] productCatalogueIds)
    {
        var response = await RequestProductsFromProductCatalogue(productCatalogueIds);
        return await ConvertToShoppingCartItems(response);
    }
    
    private ValueTask<HttpResponseMessage> RequestProductsBatchFromProductCatalogue(int start, int size)
    {
        var end = start + size;
        var queryBuilder = new QueryBuilder
        {
            { BatchStartParameter, start.ToString() },
            { BatchEndParameter, end.ToString()}
        };
        
        var productsResourceBuilder = new UriBuilder
        {
            Path = ProductsBatchUri,
            Query = queryBuilder.ToString()
        };
        var productBatchResource = productsResourceBuilder.Uri;

        return RequestFromProductCatalogue(productBatchResource);
    }

    private ValueTask<HttpResponseMessage> RequestProductsFromProductCatalogue(int[] productCatalogueIds)
    {
        var idsParameters = productCatalogueIds.Select(id => id.ToString());
        var queryBuilder = new QueryBuilder
        {
            { IdsParameter, idsParameters }
        };

        var productsResourceBuilder = new UriBuilder
        {
            Path = ProductUri,
            Query = queryBuilder.ToString()
        };
        var productResource = productsResourceBuilder.Uri;

        return RequestFromProductCatalogue(productResource);
    }
    
    private async ValueTask<HttpResponseMessage> RequestFromProductCatalogue(Uri resource)
    {
        var cacheKey = resource.ToString();
        if (cacheStore.TryGet(cacheKey) is HttpResponseMessage cachedResponse)
            return cachedResponse;

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(ProductCatalogueBaseUri);
        var response = await httpClient.GetAsync(resource);
        
        response.EnsureSuccessStatusCode();
        CacheResponse(cacheKey, response);
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