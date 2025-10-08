using Application.Cache;
using Application.Cache.Interfaces;
using Application.DTOs;
using Application.DTOs.Shop;
using Application.Exceptions;
using Application.IntegrationEvents.HttpClients.Interfaces;
using Application.IntegrationEvents.Outgoing;
using Application.Interfaces.Identity;
using Application.Interfaces.Services;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Domain.Services.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Interfaces.MessageBroker;
using System.Threading.Tasks;
namespace Application.Services
{
  

    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IProfileServiceClient _profileClient;
        private readonly ICacheService _cache;
        private readonly IEventBus _eventBus;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<ShopService> _logger;

        public ShopService(
            IShopRepository shopRepository,
            IProfileServiceClient profileClient,
            ICacheService cache,
            IEventBus eventBus,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ILogger<ShopService> logger)
        {
            _shopRepository = shopRepository;
            _profileClient = profileClient;
            _cache = cache;
            _eventBus = eventBus;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ShopDto>> GetShopByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            // Try cache first
            var cacheKey = CacheKeys.Shop(id);
            var cached = await _cache.GetAsync<ShopDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogDebug("Shop cache hit for {ShopId}", id);
                return Result<ShopDto>.Success(cached, 200);
            }

            var shop = await _shopRepository.GetByIdAsync(id, cancellationToken);
            if (shop == null)
                return Result<ShopDto>.Failure("Shop không tồn tại", 404);

            var shopDto = _mapper.Map<ShopDto>(shop);
            shopDto.FollowerCount = await _shopRepository.GetFollowerCountAsync(id, cancellationToken);

            if (_currentUserService.UserProfileId != null)
            {
                shopDto.IsFollowing = await _shopRepository.IsFollowingAsync(
                    id, _currentUserService.UserProfileId, cancellationToken);
            }

            // Get additional info from Profile Service
            try
            {
                var address = await _profileClient.GetAddressByIdAsync(shop.ShopAddressId);
                shopDto.Address = address;
            }
            catch (ServiceUnavailableException ex)
            {
                _logger.LogWarning(ex, "Profile Service unavailable when getting shop {ShopId}", id);
                // Continue without address info
            }

            // Cache for 5 minutes
            await _cache.SetAsync(cacheKey, shopDto, TimeSpan.FromMinutes(5));

            return Result<ShopDto>.Success(shopDto, 200);
        }

        public async Task<Result<ShopDto>> GetMyShopAsync(CancellationToken cancellationToken = default)
        {
            var userProfileId = _currentUserService.UserProfileId;
            if (string.IsNullOrEmpty(userProfileId))
                return Result<ShopDto>.Unauthorized();

            var shop = await _shopRepository.GetByUserProfileIdAsync(userProfileId, cancellationToken);
            if (shop == null)
                return Result<ShopDto>.Failure("Bạn chưa có shop", 404);

            var shopDto = _mapper.Map<ShopDto>(shop);
            shopDto.FollowerCount = await _shopRepository.GetFollowerCountAsync(shop.Id, cancellationToken);

            return Result<ShopDto>.Success(shopDto, 200);
        }

        public async Task<PaginatedResult<List<ShopDto>>> GetShopsAsync(
            GetShopsQuery query, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await _shopRepository.GetPaginatedAsync(
                query.PageNumber, query.PageSize, query.SearchTerm, query.Status, cancellationToken);

            var shopDtos = _mapper.Map<List<ShopDto>>(items);

            // Get follower counts in parallel
            var followerCountTasks = shopDtos.Select(async dto =>
            {
                dto.FollowerCount = await _shopRepository.GetFollowerCountAsync(dto.Id, cancellationToken);

                if (_currentUserService.UserProfileId != null)
                {
                    dto.IsFollowing = await _shopRepository.IsFollowingAsync(
                        dto.Id, _currentUserService.UserProfileId, cancellationToken);
                }
            });

            await Task.WhenAll(followerCountTasks);

            return PaginatedResult<List<ShopDto>>.Success(
                shopDtos, totalCount, query.PageNumber, query.PageSize);
        }

        public async Task<Result<ShopDto>> CreateShopAsync(
            CreateShopRequest request, CancellationToken cancellationToken = default)
        {
            var userProfileId = _currentUserService.UserProfileId;
            if (string.IsNullOrEmpty(userProfileId))
                return Result<ShopDto>.Unauthorized();

            // Check if user already has a shop
            var existingShop = await _shopRepository.GetByUserProfileIdAsync(userProfileId, cancellationToken);
            if (existingShop != null)
                return Result<ShopDto>.Failure("Bạn đã có shop rồi", 400);

            // Verify address exists in Profile Service
            try
            {
                var address = await _profileClient.GetAddressByIdAsync(request.ShopAddressId);
                if (address == null)
                    return Result<ShopDto>.Failure("Địa chỉ không hợp lệ", 400);
            }
            catch (ServiceUnavailableException)
            {
                return Result<ShopDto>.Failure("Không thể xác thực địa chỉ, vui lòng thử lại sau", 503);
            }

            var shop = _mapper.Map<Shop>(request);
            shop.ShopUserProfileId = userProfileId;
            shop.ShopStatus = true;

            // Create wallet for shop
            shop.Wallet = new WalletShop
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 0,
                ShopId = shop.Id
            };

            var createdShop = await _shopRepository.CreateAsync(shop, cancellationToken);

            // Publish event to Kafka
            await _eventBus.PublishAsync(new ShopCreatedEvent
            {
                ShopId = createdShop.Id,
                ShopName = createdShop.ShopName,
                UserProfileId = userProfileId,
                CreatedAt = DateTime.UtcNow
            });

            var shopDto = _mapper.Map<ShopDto>(createdShop);

            _logger.LogInformation("Shop {ShopId} created by user {UserId}", createdShop.Id, userProfileId);

            return Result<ShopDto>.Success(shopDto, 201);
        }

        public async Task<Result<ShopDto>> UpdateShopAsync(
            string id, UpdateShopRequest request, CancellationToken cancellationToken = default)
        {
            var shop = await _shopRepository.GetByIdAsync(id, cancellationToken);
            if (shop == null)
                return Result<ShopDto>.Failure("Shop không tồn tại", 404);

            // Check ownership
            if (shop.ShopUserProfileId != _currentUserService.UserProfileId)
                return Result<ShopDto>.Forbidden();

            _mapper.Map(request, shop);
            var updatedShop = await _shopRepository.UpdateAsync(shop, cancellationToken);

            // Invalidate cache
            await _cache.RemoveAsync(CacheKeys.Shop(id));

            var shopDto = _mapper.Map<ShopDto>(updatedShop);

            return Result<ShopDto>.Success(shopDto, 200);
        }

        public async Task<Result<bool>> DeleteShopAsync(string id, CancellationToken cancellationToken = default)
        {
            var shop = await _shopRepository.GetByIdAsync(id, cancellationToken);
            if (shop == null)
                return Result<bool>.Failure("Shop không tồn tại", 404);

            // Check ownership or admin
            if (shop.ShopUserProfileId != _currentUserService.UserProfileId &&
                !_currentUserService.IsInRole("Admin"))
                return Result<bool>.Forbidden();

            var result = await _shopRepository.DeleteAsync(id, cancellationToken);

            // Invalidate cache
            await _cache.RemoveAsync(CacheKeys.Shop(id));

            return Result<bool>.Success(result, 200);
        }

        public async Task<Result<bool>> FollowShopAsync(string shopId, CancellationToken cancellationToken = default)
        {
            var userProfileId = _currentUserService.UserProfileId;
            if (string.IsNullOrEmpty(userProfileId))
                return Result<bool>.Unauthorized();

            var shopExists = await _shopRepository.ExistsAsync(shopId, cancellationToken);
            if (!shopExists)
                return Result<bool>.Failure("Shop không tồn tại", 404);

            var result = await _shopRepository.FollowAsync(shopId, userProfileId, cancellationToken);
            if (!result)
                return Result<bool>.Failure("Bạn đã follow shop này rồi", 400);

            // Invalidate shop cache
            await _cache.RemoveAsync(CacheKeys.Shop(shopId));

            return Result<bool>.Success(true, 200);
        }

        public async Task<Result<bool>> UnfollowShopAsync(string shopId, CancellationToken cancellationToken = default)
        {
            var userProfileId = _currentUserService.UserProfileId;
            if (string.IsNullOrEmpty(userProfileId))
                return Result<bool>.Unauthorized();

            var result = await _shopRepository.UnfollowAsync(shopId, userProfileId, cancellationToken);
            if (!result)
                return Result<bool>.Failure("Bạn chưa follow shop này", 400);

            // Invalidate shop cache
            await _cache.RemoveAsync(CacheKeys.Shop(shopId));

            return Result<bool>.Success(true, 200);
        }
    
}
}

