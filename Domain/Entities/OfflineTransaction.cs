using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OfflineTransaction
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string OrderShopId { get; set; } // Reference
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
    }
}
