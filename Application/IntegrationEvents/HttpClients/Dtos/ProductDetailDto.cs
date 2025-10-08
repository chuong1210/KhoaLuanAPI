using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.HttpClients.Dtos
{
    public class ProductDetailDto
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ShopId { get; set; }
        public string CategoryId { get; set; }
        public string BrandId { get; set; }
        public bool IsActive { get; set; }
    }



  


}
