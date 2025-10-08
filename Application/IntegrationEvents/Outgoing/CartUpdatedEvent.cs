using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.Outgoing
{
    public class CartUpdatedEvent
    {
        public string CartId { get; set; }
        public string UserProfileId { get; set; }
        public int TotalItems { get; set; }
        public List<string> SkuIds { get; set; }
        public List<CartItemInfo> Items { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class CartItemInfo
    {
        public string SkuId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemRemovedEvent
    {
        public string CartId { get; set; }
        public string UserProfileId { get; set; }
        public string SkuId { get; set; }
        public DateTime RemovedAt { get; set; }
    }

    public class CartClearedEvent
    {
        public string CartId { get; set; }
        public string UserProfileId { get; set; }
        public DateTime ClearedAt { get; set; }
    }

    public class ProgressTransferCreatedEvent
    {
        public string ProgressTransferId { get; set; }
        public string OrderShopId { get; set; }
        public DateTime EstimateTime { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ProgressTransferStatusUpdatedEvent
    {
        public string ProgressTransferId { get; set; }
        public string OrderShopId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class OfflineTransactionCreatedEvent
    {
        public string TransactionId { get; set; }
        public string OrderShopId { get; set; }
        public double Amount { get; set; }
        public string ShipperId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
