using System.Net;
using Domain.Common;
using Domain.Common.Interfaces;
namespace Domain.Entities
{
    public class Shop : BaseAuditableEntity
    {
        public string ShopName { get; set; }
        public string ShopDescription { get; set; }
        public string ShopLogo { get; set; }
        public string ShopBanner { get; set; }
        public string ShopEmail { get; set; }
        public string ShopPhone { get; set; }
        public bool ShopStatus { get; set; }

        // Foreign keys - Reference IDs only
        public string ShopUserProfileId { get; set; } // Từ Profile Service
        public string ShopAddressId { get; set; } // Từ Profile Service

        // Owned by Shop Service
        public virtual WalletShop Wallet { get; set; }
        public virtual ICollection<VoucherShop> Vouchers { get; set; }
        public virtual ICollection<Follow> Followers { get; set; }
    }



}