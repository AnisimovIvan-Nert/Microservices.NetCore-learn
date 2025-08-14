using System.Net;
using System.Text.Json;
using Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;
using Microservices.NetCore.Tests.Utilities;
using Microsoft.AspNetCore.Http.Extensions;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Tests;

[Trait(Categories.TraitName, Categories.Integration.Base)]
[Trait(Categories.TraitName, Categories.Integration.InMemoryWebApp)]
public class ApplicationProductCatalogueTests : IClassFixture<CustomWebApplicationFactory>
{
    private const string BaseUri = "products";
    
    private readonly HttpClient _applicationClient;
    
    public ApplicationProductCatalogueTests(CustomWebApplicationFactory applicationFactory)
    {
        _applicationClient = applicationFactory.CreateClient();
    }
    
    [Theory]
    [InlineData(0, 5)]
    [InlineData(5, 10)]
    [InlineData(0, 1)]
    public async Task Products_Get_BatchStart_BatchEnd__ReturnsProductsInBatch(int batchStart, int batchEnd)
    {
        const string baseUri = BaseUri + "/batch";
        
        var queryBuilder = new QueryBuilder
        {
            { nameof(batchStart), batchStart.ToString() },
            { nameof(batchEnd), batchEnd.ToString() }
        };

        var uriBuilder = new UriBuilder
        {
            Path = baseUri,
            Query = queryBuilder.ToString()
        };
        var uri = uriBuilder.Uri;
        
        
        var response = await _applicationClient.GetAsync(uri);


        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var productData = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer
            .Deserialize<IEnumerable<Product>>(productData, JsonSerializerOptions.Web)?
            .ToArray();
        Assert.NotNull(products);

        var batchSize = batchEnd - batchStart;
        Assert.Equal(batchSize, products.Length);
    }
    
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(1)]
    [InlineData]
    [InlineData(int.MaxValue, int.MinValue)]
    public async Task Products_Get_Ids__ReturnsProductsByIds(params int[] ids)
    {
        var queryBuilder = new QueryBuilder();
        var idStrings = ids.Select(id => id.ToString());
        queryBuilder.Add(nameof(ids), idStrings);

        var uriBuilder = new UriBuilder
        {
            Path = BaseUri,
            Query = queryBuilder.ToString()
        };
        var uri = uriBuilder.Uri;
        
        
        var response = await _applicationClient.GetAsync(uri);
        
        
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var productData = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer
            .Deserialize<IEnumerable<Product>>(productData, JsonSerializerOptions.Web)?
            .ToArray();
        Assert.NotNull(products);

        Assert.Equal(ids.Length, products.Length);

        var productsId = products.Select(product => product.Id).ToArray();
        
        foreach (var id in ids)
            Assert.Contains(id, productsId);
    }
}