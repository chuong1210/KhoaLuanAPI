using Domain.Common;
using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Banner : BaseAuditableEntity
    {
        public string BannerName { get; set; }
        public string BannerImage { get; set; }
        public string BannerUrl { get; set; }
        public int BannerOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BannerType { get; set; } // HOME, CATEGORY, PROMOTION
        public string TargetId { get; set; } // ProductId, CategoryId (references)
    }
}
