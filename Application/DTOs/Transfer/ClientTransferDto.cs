using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationEvents.HttpClients.Dtos;
namespace Application.DTOs.Transfer
{
    public class ClientTransferDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public string LocationGoogle { get; set; }
    }
}
