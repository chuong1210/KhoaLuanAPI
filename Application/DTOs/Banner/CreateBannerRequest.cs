using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Banner
{
    public class CreateBannerRequest
    {
        [Required(ErrorMessage = "Tên banner là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Tên banner không được vượt quá 255 ký tự")]
        public string BannerName { get; set; }

        [Required(ErrorMessage = "Hình ảnh banner là bắt buộc")]
        public string BannerImage { get; set; }

        public string BannerUrl { get; set; }

        [Range(0, 999, ErrorMessage = "Thứ tự phải từ 0 đến 999")]
        public int BannerOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Loại banner là bắt buộc")]
        [RegularExpression("HOME|CATEGORY|PROMOTION", ErrorMessage = "Loại banner không hợp lệ")]
        public string BannerType { get; set; }

        public string TargetId { get; set; }
    }

}
