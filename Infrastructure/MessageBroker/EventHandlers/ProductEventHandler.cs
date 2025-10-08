using Application.Cache.Interfaces;
using Application.IntegrationEvents.Incoming;
using Infrastructure.IntegrationEvents.MessageBroker.Kafka.Interfaces;
using Domain.Services.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infrastructure.IntegrationEvents.MessageBroker.EventHandlers
{
    public class ProductEventHandler :
         IEventHandler<ProductUpdatedEvent>,
         IEventHandler<ProductDeletedEvent>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICacheService _cache;
        private readonly ILogger<ProductEventHandler> _logger;

        public ProductEventHandler(
            ICartRepository cartRepository,
            ICacheService cache,
            ILogger<ProductEventHandler> logger)
        {
            _cartRepository = cartRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(ProductUpdatedEvent @event)
        {
            _logger.LogInformation("Handling ProductUpdatedEvent for SKU {SkuId}", @event.SkuId);

            // Update cached product info in cart items
            await _cartRepository.UpdateCachedProductInfoAsync(
                @event.SkuId,
                @event.ProductName,
                @event.Image,
                @event.Price,
                @event.ShopId);

            // Invalidate related caches
            await _cache.RemoveAsync($"sku:{@event.SkuId}");
            await _cache.RemoveAsync($"product:{@event.ProductId}");

            _logger.LogInformation("Updated cached product info for SKU {SkuId}", @event.SkuId);
        }

        public async Task HandleAsync(ProductDeletedEvent @event)
        {
            _logger.LogInformation("Handling ProductDeletedEvent for SKU {SkuId}", @event.SkuId);

            // Remove items from all carts
            // Implementation depends on business requirements

            await _cache.RemoveAsync($"sku:{@event.SkuId}");
            await _cache.RemoveAsync($"product:{@event.ProductId}");

            _logger.LogInformation("Processed product deletion for SKU {SkuId}", @event.SkuId);
        }
    }

}
