using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Transfer
{

    public class CreateOfflineTransactionRequest
    {
        [Required(ErrorMessage = "Order Shop ID là bắt buộc")]
        public string OrderShopId { get; set; }

        [Required(ErrorMessage = "Số tiền là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        public double Amount { get; set; }

        public string Message { get; set; }
    }
}
