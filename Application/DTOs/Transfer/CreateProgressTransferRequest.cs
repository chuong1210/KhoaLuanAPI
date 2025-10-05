using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Transfer
{
    public class CreateProgressTransferRequest
    {
        [Required(ErrorMessage = "Order Shop ID là bắt buộc")]
        public string OrderShopId { get; set; }

        [Required(ErrorMessage = "Thời gian dự kiến là bắt buộc")]
        public DateTime EstimateTime { get; set; }

        [Required(ErrorMessage = "Địa chỉ bắt đầu là bắt buộc")]
        public string BeginAddress { get; set; }

        [Required(ErrorMessage = "Địa chỉ kết thúc là bắt buộc")]
        public string EndAddress { get; set; }

        public List<CreateProgressClientRequest> ProgressClients { get; set; }
    }
}
