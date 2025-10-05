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

        public Task<ProductDetailDto> GetProductByIdAsync(string productId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductDetailDto>> GetProductsByIdsAsync(List<string> productIds)
        {
            throw new NotImplementedException();
        }

        public async Task<SkuDetailDto> GetSkuByIdAsync(string skuId)
        {
            // Try cache first
            var cacheKey = $"sku:{skuId}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<SkuDetailDto>(cached);
            }

            try
            {
                // Call Product Service
                var response = await _httpClient.GetAsync($"/api/v1/skus/{skuId}");
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
                throw new ServiceUnavailableException("Product Service is unavailable");
            }
        }

        public async Task<List<SkuDetailDto>> GetSkusByIdsAsync(List<string> skuIds)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/skus/batch", skuIds);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<SkuDetailDto>>();
        }
    }

}
