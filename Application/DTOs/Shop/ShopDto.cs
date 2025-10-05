using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationEvents.HttpClients.Dtos;
namespace Application.DTOs.Shop
{
    // Application/DTOs/Shop/ShopDto.cs
    public class ShopDto
    {
        public string Id { get; set; }
        public string ShopName { get; set; }
        public string ShopDescription { get; set; }
        public string ShopLogo { get; set; }
        public string ShopBanner { get; set; }
        public string ShopEmail { get; set; }
        public string ShopPhone { get; set; }
        public bool ShopStatus { get; set; }
        public AddressDto Address { get; set; }
        public double WalletAmount { get; set; }
        public int FollowerCount { get; set; }
        public bool IsFollowing { get; set; }
        public DateTime CreatedDate { get; set; }
    }




}
