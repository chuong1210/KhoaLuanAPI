using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.Outgoing
{
    public class ShopCreatedEvent
    {
        public string ShopId { get; set; }
        public string ShopName { get; set; }
        public string UserProfileId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
