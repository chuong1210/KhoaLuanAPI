using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.Outgoing
{
    public class VoucherShopUsedEvent
    {
        public string VoucherShopId { get; set; }
        public string ShopId { get; set; }
        public string OrderShopId { get; set; }
        public DateTime UsedAt { get; set; }
    }

}
