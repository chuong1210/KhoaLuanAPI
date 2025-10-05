using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Banner
{
    public class UpdateBannerOrderRequest
    {
        public List<BannerOrderItem> Orders { get; set; }
    }


}
