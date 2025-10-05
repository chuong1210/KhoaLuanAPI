using Application.IntegrationEvents.Incoming;
using Application.IntegrationEvents.MessageBroker.Kafka.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services.Repositories;
namespace Application.IntegrationEvents.MessageBroker.EventHandlers
{

    public class OrderEventHandler : IEventHandler<OrderShopCompletedEvent>
    {
        //private readonly IWalletShopRepository _walletRepository;
        //private readonly IVoucherShopRepository _voucherRepository;
        private readonly ILogger<OrderEventHandler> _logger;

        public async Task HandleAsync(OrderShopCompletedEvent @event)
        {
            _logger.LogInformation("Handling OrderShopCompletedEvent for {OrderShopId}", @event.OrderShopId);

            //// Update shop wallet
            //if (@event.PaymentMethod != "COD")
            //{
            //    await _walletRepository.CreditAsync(@event.ShopId, @event.Amount,
            //        $"Payment for order {@event.OrderShopId}");
            //}

            _logger.LogInformation("Credited wallet for shop {ShopId}", @event.ShopId);
        }
    }

}
