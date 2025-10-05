using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ClientTransfer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AddressId { get; set; } // Reference only
        public string LocationGoogle { get; set; }

        // Cached address info
        public string CachedAddressLine { get; set; }
        public string CachedCity { get; set; }
        public string CachedPhone { get; set; }
    }
}
