using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Banner
{
    public class BannerDto
    {
        public string Id { get; set; }
        public string BannerName { get; set; }
        public string BannerImage { get; set; }
        public string BannerUrl { get; set; }
        public int BannerOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BannerType { get; set; }
        public string TargetId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

  
}
