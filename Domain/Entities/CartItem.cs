using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class CartItem
    {
        public string CartId { get; set; }
        public string SkuId { get; set; } // Reference to Product Service
        public int Quantity { get; set; }
        public bool IsSelected { get; set; }
        public DateTime AddedDate { get; set; }

        // Cached product info from Product Service
        public string CachedProductName { get; set; }
        public string CachedProductImage { get; set; }
        public double CachedPrice { get; set; }
        public string CachedShopId { get; set; }
        public DateTime CachedAt { get; set; }

        public virtual Cart Cart { get; set; }
    }

}
