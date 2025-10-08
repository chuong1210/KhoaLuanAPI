using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Policy
{
    public class GetPoliciesQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public string PolicyType { get; set; }
        public bool? IsActive { get; set; }
        public string ShopId { get; set; }
    }
}
