using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Policy
{
    public class PolicySummaryDto
    {
        public string Id { get; set; }
        public string PolicyName { get; set; }
        public string PolicyType { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
}
