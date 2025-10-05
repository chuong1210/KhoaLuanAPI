using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WalletShop
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string ShopId { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<ShopTransaction> Transactions { get; set; }
    }
}
