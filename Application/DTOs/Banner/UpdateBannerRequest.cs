using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Banner
{

    public class UpdateBannerRequest
    {
        [MaxLength(255, ErrorMessage = "Tên banner không được vượt quá 255 ký tự")]
        public string BannerName { get; set; }

        public string BannerImage { get; set; }

        public string BannerUrl { get; set; }

        [Range(0, 999, ErrorMessage = "Thứ tự phải từ 0 đến 999")]
        public int? BannerOrder { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string TargetId { get; set; }
    }

}
