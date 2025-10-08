using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.IntegrationEvents.HttpClients.Dtos;
using Application.IntegrationEvents.HttpClients.Interfaces;
using Application.Exceptions;
using Application.Responses;
namespace Application.IntegrationEvents.HttpClients
{
    public class ProductServiceClient : IProductServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductServiceClient> _logger;

        public ProductServiceClient(
            HttpClient httpClient,
            IDistributedCache cache,
            ILogger<ProductServiceClient> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<SkuDetailDto> GetSkuByIdAsync(string skuId)
        {
            // Try cache first
            var cacheKey = $"sku:{skuId}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
            {
                _logger.LogDebug("SKU cache hit for {SkuId}", skuId);
                return JsonSerializer.Deserialize<SkuDetailDto>(cached);
            }

            try
            {
                _logger.LogInformation("Calling Product Service to get SKU {SkuId}", skuId);

                var response = await _httpClient.GetAsync($"/api/v1/skus/{skuId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("SKU {SkuId} not found in Product Service", skuId);
                    return null;
                }

                response.EnsureSuccessStatusCode();

                var sku = await response.Content.ReadFromJsonAsync<SkuDetailDto>();

                // Cache for 5 minutes
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(sku), options);

                return sku;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get SKU {SkuId} from Product Service", skuId);
                throw new ServiceUnavailableException("Product Service is unavailable", ex);
            }
        }

        public async Task<List<SkuDetailDto>> GetSkusByIdsAsync(List<string> skuIds)
        {
            try
            {
                _logger.LogInformation("Calling Product Service to get {Count} SKUs", skuIds.Count);

                var response = await _httpClient.PostAsJsonAsync("/api/v1/skus/batch", skuIds);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<SkuDetailDto>>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get SKUs from Product Service");
                throw new ServiceUnavailableException("Product Service is unavailable", ex);
            }
        }

        public async Task<bool> CheckSkuStockAsync(string skuId, int quantity)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/v1/skus/{skuId}/stock/{quantity}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to check stock for SKU {SkuId}", skuId);
                return false;
            }
        }

        public async Task<bool> ReserveStockAsync(string skuId, int quantity)
        {
            try
            {
                var request = new { SkuId = skuId, Quantity = quantity };
                var response = await _httpClient.PostAsJsonAsync("/api/v1/skus/reserve", request);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to reserve stock for SKU {SkuId}", skuId);
                return false;
            }
        }

        public async Task<bool> ReleaseStockAsync(string skuId, int quantity)
        {
            try
            {
                var request = new { SkuId = skuId, Quantity = quantity };
                var response = await _httpClient.PostAsJsonAsync("/api/v1/skus/release", request);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to release stock for SKU {SkuId}", skuId);
                return false;
            }
        }

        public async Task<ProductDetailDto> GetProductByIdAsync(string productId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/v1/products/{productId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ProductDetailDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get product {ProductId}", productId);
                throw new ServiceUnavailableException("Product Service is unavailable", ex);
            }
        }

        public async Task<List<ProductDetailDto>> GetProductsByShopIdAsync(string shopId, int pageNumber, int pageSize)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"/api/v1/products?shopId={shopId}&pageNumber={pageNumber}&pageSize={pageSize}");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<PaginatedResult<List<ProductDetailDto>>>();
                return result.result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get products for shop {ShopId}", shopId);
                return new List<ProductDetailDto>();
            }
        }

        public async Task<bool> ProductExistsAsync(string productId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/v1/products/{productId}/exists");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(string categoryId)
        {
            var cacheKey = $"category:{categoryId}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<CategoryDto>(cached);

            try
            {
                var response = await _httpClient.GetAsync($"/api/v1/categories/{categoryId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                var category = await response.Content.ReadFromJsonAsync<CategoryDto>();

                // Cache for 30 minutes
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(category),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    });

                return category;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get category {CategoryId}", categoryId);
                return null;
            }
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/v1/categories");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get categories");
                return new List<CategoryDto>();
            }
        }
    }

}
