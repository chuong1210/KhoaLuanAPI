using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Cart
{
    public class ShopCartDto
    {
        public string ShopId { get; set; }
        public string ShopName { get; set; }
        public List<CartItemDto> Items { get; set; }
        public double SubTotal { get; set; }
        public bool IsAllSelected { get; set; }
    }

  
}
