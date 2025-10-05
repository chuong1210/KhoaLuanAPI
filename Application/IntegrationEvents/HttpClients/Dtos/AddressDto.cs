using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.HttpClients.Dtos
{
    public class AddressDto
    {
        public string Id { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Phone { get; set; }
        public string ReceiverName { get; set; }
    }
}
