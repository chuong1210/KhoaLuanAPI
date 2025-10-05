using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Cart
{
    public class UpdateCartItemSelectionRequest
    {
        [Required(ErrorMessage = "Trạng thái chọn là bắt buộc")]
        public bool IsSelected { get; set; }
    }


}
