using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Cart
{

    public class AddToCartRequest
    {
        [Required(ErrorMessage = "SKU ID là bắt buộc")]
        public string SkuId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, 999, ErrorMessage = "Số lượng phải từ 1 đến 999")]
        public int Quantity { get; set; } = 1;
    }

}
