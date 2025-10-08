using Application.IntegrationEvents.Incoming;
using Infrastructure.IntegrationEvents.MessageBroker.Kafka.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services.Repositories;
namespace Infrastructure.IntegrationEvents.MessageBroker.EventHandlers
{

    public class OrderEventHandler :
          IEventHandler<OrderShopCompletedEvent>,
          IEventHandler<OrderShopCancelledEvent>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IVoucherShopRepository _voucherRepository;
        private readonly ILogger<OrderEventHandler> _logger;

        public OrderEventHandler(
            IShopRepository shopRepository,
            IVoucherShopRepository voucherRepository,
            ILogger<OrderEventHandler> logger)
        {
            _shopRepository = shopRepository;
            _voucherRepository = voucherRepository;
            _logger = logger;
        }

        public async Task HandleAsync(OrderShopCompletedEvent @event)
        {
            _logger.LogInformation("Handling OrderShopCompletedEvent for {OrderShopId}", @event.OrderShopId);

            // Update shop wallet if payment is not COD
            if (@event.PaymentMethod != "COD")
            {
                // Logic to credit shop wallet
                _logger.LogInformation("Crediting wallet for shop {ShopId} amount {Amount}",
                    @event.ShopId, @event.Amount);
            }
        }

        public async Task HandleAsync(OrderShopCancelledEvent @event)
        {
            _logger.LogInformation("Handling OrderShopCancelledEvent for {OrderShopId}", @event.OrderShopId);

            // Return voucher usage if applicable
            if (!string.IsNullOrEmpty(@event.VoucherShopId))
            {
                // Decrement voucher usage count
                _logger.LogInformation("Returning voucher {VoucherShopId} for cancelled order",
                    @event.VoucherShopId);
            }
        }
    }

}
