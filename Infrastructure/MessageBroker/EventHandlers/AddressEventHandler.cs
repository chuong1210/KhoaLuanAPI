using Application.Cache.Interfaces;
using Application.IntegrationEvents.Incoming;
using Infrastructure.IntegrationEvents.MessageBroker.Kafka.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IntegrationEvents.MessageBroker.EventHandlers
{
    public class AddressEventHandler : IEventHandler<AddressUpdatedEvent>
    {
        private readonly ShopDbContext _context;
        private readonly ICacheService _cache;
        private readonly ILogger<AddressEventHandler> _logger;

        public AddressEventHandler(
            ShopDbContext context,
            ICacheService cache,
            ILogger<AddressEventHandler> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(AddressUpdatedEvent @event)
        {
            _logger.LogInformation("Handling AddressUpdatedEvent for {AddressId}", @event.AddressId);

            // Update cached address in ClientTransfer
            var clientTransfers = await _context.ClientTransfers
                .Where(ct => ct.AddressId == @event.AddressId)
                .ToListAsync();

            foreach (var ct in clientTransfers)
            {
                ct.CachedAddressLine = @event.AddressLine;
                ct.CachedCity = @event.City;
                ct.CachedPhone = @event.PhoneNumber;
            }

            await _context.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync($"address:{@event.AddressId}");

            _logger.LogInformation("Updated cached address for {Count} client transfers", clientTransfers.Count);
        }
    }

}
