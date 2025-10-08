using Application.Exceptions;
using Application.IntegrationEvents.HttpClients.Dtos;
using Application.IntegrationEvents.HttpClients.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.HttpClients
{
    public class ProfileServiceClient : IProfileServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProfileServiceClient> _logger;

        public ProfileServiceClient(
            HttpClient httpClient,
            IDistributedCache cache,
            ILogger<ProfileServiceClient> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<UserProfileDto> GetUserProfileByIdAsync(string userProfileId)
        {
            var cacheKey = $"profile:{userProfileId}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<UserProfileDto>(cached);

            try
            {
                var response = await _httpClient.GetAsync($"/api/v1/profiles/{userProfileId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                var profile = await response.Content.ReadFromJsonAsync<UserProfileDto>();

                // Cache for 10 minutes
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(profile),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });

                return profile;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get user profile {UserProfileId}", userProfileId);
                throw new ServiceUnavailableException("Profile Service is unavailable", ex);
            }
        }

        public async Task<bool> UserProfileExistsAsync(string userProfileId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/v1/profiles/{userProfileId}/exists");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public async Task<AddressDto> GetAddressByIdAsync(string addressId)
        {
            var cacheKey = $"address:{addressId}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<AddressDto>(cached);

            try
            {
                _logger.LogInformation("Calling Profile Service to get address {AddressId}", addressId);

                var response = await _httpClient.GetAsync($"/api/v1/addresses/{addressId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

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
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get address {AddressId}", addressId);
                throw new ServiceUnavailableException("Profile Service is unavailable", ex);
            }
        }

        public async Task<List<AddressDto>> GetAddressesByUserProfileIdAsync(string userProfileId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/v1/addresses/user/{userProfileId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<AddressDto>>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get addresses for user {UserProfileId}", userProfileId);
                return new List<AddressDto>();
            }
        }

        public async Task<AddressDto> GetDefaultAddressAsync(string userProfileId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/v1/addresses/user/{userProfileId}/default");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<AddressDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get default address for user {UserProfileId}", userProfileId);
                return null;
            }
        }

        public async Task<bool> ValidateAddressAsync(string addressId, string userProfileId)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"/api/v1/addresses/{addressId}/validate?userProfileId={userProfileId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }



}
