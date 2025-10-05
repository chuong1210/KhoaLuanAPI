using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEvents.HttpClients.Dtos
{
    public class AttributeOptionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string AttributeName { get; set; } = string.Empty;  // e.g., "Size", "Color"
    }
}
