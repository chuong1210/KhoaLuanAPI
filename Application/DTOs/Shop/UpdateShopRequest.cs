using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Shop
{
    public class UpdateShopRequest
    {
        public string ShopName { get; set; }
        public string ShopDescription { get; set; }
        public string ShopLogo { get; set; }
        public string ShopBanner { get; set; }
        public string ShopEmail { get; set; }
        public string ShopPhone { get; set; }
        public bool ShopStatus { get; set; }
    }

}
