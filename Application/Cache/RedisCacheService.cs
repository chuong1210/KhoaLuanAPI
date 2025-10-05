using Application.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var cached = await _cache.GetStringAsync(key);
                return cached == null ? default : JsonSerializer.Deserialize<T>(cached);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache get failed for key {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var serialized = JsonSerializer.Serialize(value);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
                };
                await _cache.SetStringAsync(key, serialized, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache set failed for key {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache remove failed for key {Key}", key);
            }
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            // Implementation depends on Redis version and library
            // Can use Redis SCAN command with pattern matching
        }
    }
}
