using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Policy
{
    public class PublishPolicyRequest
    {
        [Required]
        public DateTime EffectiveDate { get; set; }
    }
}
