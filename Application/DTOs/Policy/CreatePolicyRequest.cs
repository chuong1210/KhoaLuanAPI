using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Policy
{
    public class CreatePolicyRequest
    {
        [Required(ErrorMessage = "Tên chính sách là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Tên chính sách không được vượt quá 255 ký tự")]
        public string PolicyName { get; set; }

        [Required(ErrorMessage = "Nội dung chính sách là bắt buộc")]
        public string PolicyContent { get; set; }

        [Required(ErrorMessage = "Loại chính sách là bắt buộc")]
        [RegularExpression("TERMS|PRIVACY|RETURN|WARRANTY|SHIPPING",
            ErrorMessage = "Loại chính sách không hợp lệ")]
        public string PolicyType { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string ShopId { get; set; } // NULL for system-wide policies
    }
}
