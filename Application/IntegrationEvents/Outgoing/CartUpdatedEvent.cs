using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.Outgoing
{
    public class CartUpdatedEvent
    {
        public string CartId { get; set; }
        public string UserProfileId { get; set; }
        public int TotalItems { get; set; }
        public List<string> SkuIds { get; set; }
    }
}
