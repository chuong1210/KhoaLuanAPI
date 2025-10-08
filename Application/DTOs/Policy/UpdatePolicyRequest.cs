using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Policy
{
    public class UpdatePolicyRequest
    {
        [MaxLength(255)]
        public string PolicyName { get; set; }

        public string PolicyContent { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? EffectiveDate { get; set; }
    }
}
