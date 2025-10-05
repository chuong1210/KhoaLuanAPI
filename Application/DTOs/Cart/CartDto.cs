using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Cart
{
    public class CartDto
    {
        public string Id { get; set; }
        public List<CartItemDto> Items { get; set; }
        public int TotalItems { get; set; }
        public double TotalPrice { get; set; }
        public double SelectedTotalPrice { get; set; }
        public Dictionary<string, ShopCartDto> ShopGroups { get; set; }
    }

  

}
