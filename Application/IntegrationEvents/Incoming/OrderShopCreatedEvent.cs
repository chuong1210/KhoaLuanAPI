using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.Incoming
{
    public class OrderShopCreatedEvent
    {
        public string OrderShopId { get; set; }
        public string ShopId { get; set; }
        public double Total { get; set; }
        public string VoucherShopId { get; set; }
    }

    public class OrderShopCompletedEvent
    {
        public string OrderShopId { get; set; }
        public string ShopId { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class OrderItemData
    {
        public string SkuId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }



    public class OrderShopCancelledEvent
    {
        public string OrderShopId { get; set; }
        public string ShopId { get; set; }
        public string VoucherShopId { get; set; }
        public string Reason { get; set; }
        public DateTime CancelledAt { get; set; }
    }
}
