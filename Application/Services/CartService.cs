using Application.Cache;
using Application.Cache.Interfaces;
using Application.DTOs.Cart;
using Application.Exceptions;
using Application.IntegrationEvents.HttpClients.Dtos;
using Application.IntegrationEvents.HttpClients.Interfaces;
using Application.IntegrationEvents.MessageBroker.Kafka.Interfaces;
using Application.IntegrationEvents.Outgoing;
using Application.Interfaces.Identity;
using Application.Interfaces.Services;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Domain.Services.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductServiceClient _productClient; // MỚI: HTTP Client
        private readonly ICacheService _cache; // MỚI: Redis cache
        private readonly IEventBus _eventBus; // MỚI: Kafka producer
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(
            ICartRepository cartRepository,
            IProductServiceClient productClient,
            ICacheService cache,
            IEventBus eventBus,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _productClient = productClient;
            _cache = cache;
            _eventBus = eventBus;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CartDto>> AddToCartAsync(AddToCartRequest request, CancellationToken cancellationToken = default)
        {
            var userProfileId = _currentUserService.UserProfileId;
            if (string.IsNullOrEmpty(userProfileId))
                return Result<CartDto>.Unauthorized();

            // MỚI: Gọi Product Service qua HTTP để lấy SKU info
            SkuDetailDto sku;
            try
            {
                sku = await _productClient.GetSkuByIdAsync(request.SkuId);
            }
            catch (ServiceUnavailableException ex)
            {
                _logger.LogError(ex, "Product Service unavailable");
                return Result<CartDto>.Failure("Dịch vụ sản phẩm tạm thời không khả dụng, vui lòng thử lại sau", 503);
            }

            if (sku == null)
                return Result<CartDto>.Failure("Sản phẩm không tồn tại", 404);

            if (sku.Stock < request.Quantity)
                return Result<CartDto>.Failure($"Chỉ còn {sku.Stock} sản phẩm trong kho", 400);

            var cart = await _cartRepository.GetOrCreateByUserProfileIdAsync(userProfileId, cancellationToken);

            // MỚI: Cache product info vào CartItem
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                SkuId = request.SkuId,
                Quantity = request.Quantity,
                IsSelected = true,
                AddedDate = DateTime.UtcNow,
                // Cache product data
                CachedProductName = sku.ProductName,
                CachedProductImage = sku.ProductImage,
                CachedPrice = sku.Price,
                CachedShopId = sku.ShopId,
                CachedAt = DateTime.UtcNow
            };

            await _cartRepository.AddItemAsync(cartItem, cancellationToken);

            // MỚI: Publish event to Kafka
            await _eventBus.PublishAsync(new CartUpdatedEvent
            {
                CartId = cart.Id,
                UserProfileId = userProfileId,
                TotalItems = cart.Items.Count,
                SkuIds = cart.Items.Select(i => i.SkuId).ToList()
            });

            // MỚI: Invalidate cache
            await _cache.RemoveAsync(CacheKeys.Cart(userProfileId));

            _logger.LogInformation("Added SKU {SkuId} to cart for user {UserId}", request.SkuId, userProfileId);

            return await GetMyCartAsync(cancellationToken);
        }

        public Task<Result<bool>> BatchUpdateSelectionAsync(BatchUpdateSelectionRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> ClearCartAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<int>> GetCartItemCountAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<CartDto>> GetMyCartAsync(CancellationToken cancellationToken = default)
        {
            var userProfileId = _currentUserService.UserProfileId;
            if (string.IsNullOrEmpty(userProfileId))
                return Result<CartDto>.Unauthorized();

            // MỚI: Try cache first
            var cacheKey = CacheKeys.Cart(userProfileId);
            var cachedCart = await _cache.GetAsync<CartDto>(cacheKey);

            if (cachedCart != null)
            {
                _logger.LogDebug("Cart cache hit for user {UserId}", userProfileId);
                return Result<CartDto>.Success(cachedCart, 200);
            }

            var cart = await _cartRepository.GetByUserProfileIdAsync(userProfileId, cancellationToken);

            if (cart == null || !cart.Items.Any())
            {
                return Result<CartDto>.Success(new CartDto
                {
                    Id = cart?.Id ?? string.Empty,
                    Items = new List<CartItemDto>(),
                    TotalItems = 0,
                    TotalPrice = 0
                }, 200);
            }

            // MỚI: Check stale cache và refresh nếu cần
            var staleItems = cart.Items
                .Where(i => DateTime.UtcNow - i.CachedAt > TimeSpan.FromMinutes(5))
                .ToList();

            if (staleItems.Any())
            {
                _logger.LogInformation("Refreshing stale cache for {Count} items", staleItems.Count);

                var skuIds = staleItems.Select(i => i.SkuId).ToList();
                var freshSkus = await _productClient.GetSkusByIdsAsync(skuIds);

                foreach (var item in staleItems)
                {
                    var freshSku = freshSkus.FirstOrDefault(s => s.Id == item.SkuId);
                    if (freshSku != null)
                    {
                        item.CachedProductName = freshSku.ProductName;
                        item.CachedProductImage = freshSku.ProductImage;
                        item.CachedPrice = freshSku.Price;
                        item.CachedShopId = freshSku.ShopId;
                        item.CachedAt = DateTime.UtcNow;
                    }
                }

                await _cartRepository.UpdateAsync(cart, cancellationToken);
            }

            var cartDto = _mapper.Map<CartDto>(cart);

            // Group by shop
            cartDto.ShopGroups = cart.Items
                .GroupBy(i => i.CachedShopId)
                .ToDictionary(
                    g => g.Key,
                    g => new ShopCartDto
                    {
                        ShopId = g.Key,
                        Items = _mapper.Map<List<CartItemDto>>(g.ToList()),
                        SubTotal = g.Sum(i => i.CachedPrice * i.Quantity),
                        IsAllSelected = g.All(i => i.IsSelected)
                    });

            // MỚI: Cache for 2 minutes
            await _cache.SetAsync(cacheKey, cartDto, TimeSpan.FromMinutes(2));

            return Result<CartDto>.Success(cartDto, 200);
        }

        public Task<Result<bool>> RemoveCartItemAsync(string skuId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<CartDto>> UpdateCartItemAsync(string skuId, UpdateCartItemRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> UpdateItemSelectionAsync(string skuId, UpdateCartItemSelectionRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

