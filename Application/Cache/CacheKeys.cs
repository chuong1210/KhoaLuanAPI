using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cache
{
    public static class CacheKeys
    {
        public static string Cart(string userProfileId) => $"cart:{userProfileId}";
        public static string Shop(string shopId) => $"shop:{shopId}";
        public static string Product(string productId) => $"product:{productId}";
        public static string Sku(string skuId) => $"sku:{skuId}";
        public static string UserProfile(string userProfileId) => $"profile:{userProfileId}";
        public static string Address(string addressId) => $"address:{addressId}";
    }
}
