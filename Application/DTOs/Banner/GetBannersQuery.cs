using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Banner
{

    public class GetBannersQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public string BannerType { get; set; }
        public bool? IsActive { get; set; }
    }


}
