using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.Incoming
{
    public class AddressUpdatedEvent
    {
        public string AddressId { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }
}
