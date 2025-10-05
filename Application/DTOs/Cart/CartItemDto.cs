using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Cart
{
    public class CartItemDto
    {
        public string SkuId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsSelected { get; set; }
        public string ShopId { get; set; }
        public string ShopName { get; set; }
        public List<AttributeOptionDto> Attributes { get; set; }
        public int Stock { get; set; }
        public DateTime AddedDate { get; set; }
    }

   
}
