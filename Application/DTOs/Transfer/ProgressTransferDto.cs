using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Transfer
{
    public class ProgressTransferDto
    {
        public string Id { get; set; }
        public string OrderShopId { get; set; }
        public DateTime EstimateTime { get; set; }
        public string Status { get; set; }
        public string BeginAddress { get; set; }
        public string EndAddress { get; set; }
        public List<ProgressClientDto> ProgressClients { get; set; }
    }
}
