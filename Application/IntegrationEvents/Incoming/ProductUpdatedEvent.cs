using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.Incoming
{
    public class ProductUpdatedEvent
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string ShopId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ProductDeletedEvent
    {
        public string ProductId { get; set; }
        public string SkuId { get; set; }
    }

    // From Profile Service
    public class UserProfileCreatedEvent
    {
        public string UserProfileId { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
    }
}
