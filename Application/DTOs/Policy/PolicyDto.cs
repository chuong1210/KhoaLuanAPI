using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Policy
{
    public class PolicyDto
    {
        public string Id { get; set; }
        public string PolicyName { get; set; }
        public string PolicyContent { get; set; }
        public string PolicyType { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string ShopId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
