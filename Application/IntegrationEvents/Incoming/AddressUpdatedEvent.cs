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
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
