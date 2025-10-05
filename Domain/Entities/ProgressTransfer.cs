using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enums;
namespace Domain.Entities
{
    public class ProgressTransfer
    {
        public string Id { get; set; }
        public string OrderShopId { get; set; } // Reference to Order Service
        public DateTime EstimateTime { get; set; }
        public string Status { get; set; } // COMING, ARRIVED, GONE
        public string BeginAddress { get; set; }
        public string EndAddress { get; set; }

        public virtual ICollection<ProgressClient> ProgressClients { get; set; }
    }
}
