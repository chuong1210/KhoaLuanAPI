using Domain.Common;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShopTransaction
    {
        public string Id { get; set; }
        public string Status { get; set; } // SUCCESS, PENDING, FAILED
        public bool Type { get; set; } // true: credit, false: debit
        public double Amount { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public string WalletShopId { get; set; }
        public string OrderShopId { get; set; } // Reference to Order Service

        public virtual WalletShop WalletShop { get; set; }
    }
}
