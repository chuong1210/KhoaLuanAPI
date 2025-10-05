using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Transfer
{
    public class UpdateProgressTransferStatusRequest
    {
        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [RegularExpression("COMING|ARRIVED|GONE", ErrorMessage = "Trạng thái không hợp lệ")]
        public string Status { get; set; }
    }

}
