using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.IntegrationEvents.HttpClients.Dtos;
using Application.IntegrationEvents.HttpClients.Interfaces;

namespace Application.IntegrationEvents.HttpClients
{
    public class ProfileServiceClient : IProfileServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        public ProfileServiceClient(HttpClient httpClient, IDistributedCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<AddressDto> GetAddressByIdAsync(string addressId)
        {
            var cacheKey = $"address:{addressId}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<AddressDto>(cached);
            }

            var response = await _httpClient.GetAsync($"/api/v1/addresses/{addressId}");
            response.EnsureSuccessStatusCode();

            var address = await response.Content.ReadFromJsonAsync<AddressDto>();

            // Cache for 10 minutes
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(address),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

            return address;
        }
    }



}
