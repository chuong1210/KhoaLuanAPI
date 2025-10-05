using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class VoucherShop : BaseAuditableEntity
    {
        public string Name { get; set; }
        public float Discount { get; set; }
        public DateTime StartAvailable { get; set; }
        public DateTime End { get; set; }
        public double MinSupport { get; set; }
        public double MaxDiscount { get; set; }
        public string ShopId { get; set; }
        public string Image { get; set; }
        public string CategoryId { get; set; } // Reference only
        public int Quantity { get; set; }
        public int Used { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }

        public virtual Shop Shop { get; set; }
    }
}
