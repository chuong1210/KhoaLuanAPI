using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.HttpClients.Dtos
{
    public class SkuDetailDto
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ShopId { get; set; }
    }
}
