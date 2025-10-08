using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Policy : BaseAuditableEntity
    {
        public string PolicyName { get; set; }
        public string PolicyContent { get; set; }
        public string PolicyType { get; set; } // TERMS, PRIVACY, RETURN, WARRANTY, SHIPPING
        public bool IsActive { get; set; }
        public int Version { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string ShopId { get; set; } // NULL for system-wide policies
    }
}
