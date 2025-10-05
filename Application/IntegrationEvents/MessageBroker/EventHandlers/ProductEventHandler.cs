using Application.IntegrationEvents.Incoming;
using Domain.Services.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationEvents.MessageBroker.Kafka.Interfaces;
namespace Application.IntegrationEvents.MessageBroker.EventHandlers
{
    public class ProductEventHandler : IEventHandler<ProductUpdatedEvent>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductEventHandler> _logger;

        public ProductEventHandler(
            ICartRepository cartRepository,
            IDistributedCache cache,
            ILogger<ProductEventHandler> logger)
        {
            _cartRepository = cartRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(ProductUpdatedEvent @event)
        {
        //    _logger.LogInformation("Handling ProductUpdatedEvent for Product {ProductId}", @event.ProductId);

        //    // Update cached product info in cart items
        //    await _cartRepository.UpdateCachedProductInfoAsync(
        //        @event.ProductId,
        //        @event.ProductName,
        //        @event.Image,
        //        @event.Price,
        //        @event.ShopId);

        //    // Invalidate cache
        //    await _cache.RemoveAsync($"sku:{@event.ProductId}");

        //    _logger.LogInformation("Updated cached product info for {ProductId}", @event.ProductId);
        }
    }

}
